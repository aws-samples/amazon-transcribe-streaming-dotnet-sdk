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
            btnRecordMike = new Button();
            lblInputLanguage = new Label();
            cbxLanguages = new ComboBox();
            btnRecordSpeakers = new Button();
            rtbTranscriptionMike = new RichTextBox();
            label1 = new Label();
            cbxInputDevices = new ComboBox();
            cbxSpkrLanguages = new ComboBox();
            label2 = new Label();
            SuspendLayout();
            // 
            // rtbTranscription
            // 
            rtbTranscription.Location = new Point(411, 167);
            rtbTranscription.Name = "rtbTranscription";
            rtbTranscription.Size = new Size(377, 271);
            rtbTranscription.TabIndex = 0;
            rtbTranscription.Text = "";
            // 
            // btnRecordMike
            // 
            btnRecordMike.Location = new Point(14, 19);
            btnRecordMike.Name = "btnRecordMike";
            btnRecordMike.Size = new Size(137, 45);
            btnRecordMike.TabIndex = 1;
            btnRecordMike.Text = "Start Mic Transcription";
            btnRecordMike.UseVisualStyleBackColor = true;
            // 
            // lblInputLanguage
            // 
            lblInputLanguage.AutoSize = true;
            lblInputLanguage.Location = new Point(14, 77);
            lblInputLanguage.Name = "lblInputLanguage";
            lblInputLanguage.Size = new Size(82, 15);
            lblInputLanguage.TabIndex = 2;
            lblInputLanguage.Text = "Mic Language";
            // 
            // cbxLanguages
            // 
            cbxLanguages.DropDownStyle = ComboBoxStyle.DropDownList;
            cbxLanguages.FormattingEnabled = true;
            cbxLanguages.Location = new Point(110, 74);
            cbxLanguages.Name = "cbxLanguages";
            cbxLanguages.Size = new Size(281, 23);
            cbxLanguages.TabIndex = 3;
            cbxLanguages.SelectedIndexChanged += cbxLanguages_SelectedIndexChanged;
            // 
            // btnRecordSpeakers
            // 
            btnRecordSpeakers.Location = new Point(651, 19);
            btnRecordSpeakers.Name = "btnRecordSpeakers";
            btnRecordSpeakers.Size = new Size(137, 45);
            btnRecordSpeakers.TabIndex = 4;
            btnRecordSpeakers.Text = "Start Speakers Transcription";
            btnRecordSpeakers.UseVisualStyleBackColor = true;
            // 
            // rtbTranscriptionMike
            // 
            rtbTranscriptionMike.Location = new Point(14, 167);
            rtbTranscriptionMike.Name = "rtbTranscriptionMike";
            rtbTranscriptionMike.Size = new Size(377, 271);
            rtbTranscriptionMike.TabIndex = 5;
            rtbTranscriptionMike.Text = "";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(14, 120);
            label1.Name = "label1";
            label1.Size = new Size(73, 15);
            label1.TabIndex = 6;
            label1.Text = "Input Device";
            // 
            // cbxInputDevices
            // 
            cbxInputDevices.DropDownStyle = ComboBoxStyle.DropDownList;
            cbxInputDevices.FormattingEnabled = true;
            cbxInputDevices.Location = new Point(110, 117);
            cbxInputDevices.Name = "cbxInputDevices";
            cbxInputDevices.Size = new Size(281, 23);
            cbxInputDevices.TabIndex = 7;
            // 
            // cbxSpkrLanguages
            // 
            cbxSpkrLanguages.DropDownStyle = ComboBoxStyle.DropDownList;
            cbxSpkrLanguages.FormattingEnabled = true;
            cbxSpkrLanguages.Location = new Point(507, 77);
            cbxSpkrLanguages.Name = "cbxSpkrLanguages";
            cbxSpkrLanguages.Size = new Size(281, 23);
            cbxSpkrLanguages.TabIndex = 9;
            cbxSpkrLanguages.SelectedIndexChanged += cbxSpkrLanguages_SelectedIndexChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(411, 80);
            label2.Name = "label2";
            label2.Size = new Size(108, 15);
            label2.TabIndex = 8;
            label2.Text = "Speakers Language";
            
            // 
            // TranscribeApp
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(cbxSpkrLanguages);
            Controls.Add(label2);
            Controls.Add(cbxInputDevices);
            Controls.Add(label1);
            Controls.Add(rtbTranscriptionMike);
            Controls.Add(btnRecordSpeakers);
            Controls.Add(cbxLanguages);
            Controls.Add(lblInputLanguage);
            Controls.Add(btnRecordMike);
            Controls.Add(rtbTranscription);
            Name = "TranscribeApp";
            Text = "Transcribe";
            Load += TranscribeApp_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private RichTextBox rtbTranscription;
        private Button btnRecordMike;
        private Label lblInputLanguage;
        private ComboBox cbxLanguages;
        private Button btnRecordSpeakers;
        private RichTextBox rtbTranscriptionMike;
        private Label label1;
        private ComboBox cbxInputDevices;
        private ComboBox cbxSpkrLanguages;
        private Label label2;
    }
}
