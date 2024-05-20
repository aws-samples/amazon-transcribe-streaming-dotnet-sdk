namespace Amazon.TranscribeStreamingService.Example
{
    partial class TranscribeApp
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
            rtbTranscription = new RichTextBox();
            btnStartTranscription = new Button();
            lblInputLanguage = new Label();
            cbxLanguages = new ComboBox();
            SuspendLayout();
            // 
            // rtbTranscription
            // 
            rtbTranscription.Location = new Point(15, 91);
            rtbTranscription.Name = "rtbTranscription";
            rtbTranscription.Size = new Size(773, 347);
            rtbTranscription.TabIndex = 0;
            rtbTranscription.Text = "";
            // 
            // btnStartTranscription
            // 
            btnStartTranscription.Location = new Point(14, 19);
            btnStartTranscription.Name = "btnStartTranscription";
            btnStartTranscription.Size = new Size(137, 45);
            btnStartTranscription.TabIndex = 1;
            btnStartTranscription.Text = "Start Transcription";
            btnStartTranscription.UseVisualStyleBackColor = true;
            // 
            // lblInputLanguage
            // 
            lblInputLanguage.AutoSize = true;
            lblInputLanguage.Location = new Point(205, 34);
            lblInputLanguage.Name = "lblInputLanguage";
            lblInputLanguage.Size = new Size(90, 15);
            lblInputLanguage.TabIndex = 2;
            lblInputLanguage.Text = "Input Language";
            // 
            // cbxLanguages
            // 
            cbxLanguages.DropDownStyle = ComboBoxStyle.DropDownList;
            cbxLanguages.FormattingEnabled = true;
            cbxLanguages.Location = new Point(312, 31);
            cbxLanguages.Name = "cbxLanguages";
            cbxLanguages.Size = new Size(242, 23);
            cbxLanguages.TabIndex = 3;
            cbxLanguages.SelectedIndexChanged += cbxLanguages_SelectedIndexChanged;
            // 
            // TranscribeApp
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(cbxLanguages);
            Controls.Add(lblInputLanguage);
            Controls.Add(btnStartTranscription);
            Controls.Add(rtbTranscription);
            Name = "TranscribeApp";
            Text = "Transcribe";
            Load += TranscribeApp_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private RichTextBox rtbTranscription;
        private Button btnStartTranscription;
        private Label lblInputLanguage;
        private ComboBox cbxLanguages;
    }
}
