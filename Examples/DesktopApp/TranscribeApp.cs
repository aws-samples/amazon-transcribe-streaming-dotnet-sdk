using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

using Amazon;
using Amazon.TranscribeService;
using Amazon.TranscribeStreamingService;
using Amazon.TranscribeStreamingService.Models;

using NAudio;
using NAudio.Wave;




namespace Amazon.TranscribeStreamingService.Example
{
    public partial class TranscribeApp : Form
    {

        // Audio recording components
        private WaveInEvent? _waveIn;
        private MemoryStream? _memoryStream;
        private Task? _sendAudioTask;
        private Task? _receiveTranscriptionTask;

        // Recording state
        private bool _isRecording = false;

        // Text variables for holding transcriptions
        private static string TranscribedText = "";
        private static string result = "";
        private static string currenttext = "";

        // Amazon Transcribe Streaming Client
        private static AmazonTranscribeStreamingClient client;

        // Default language code
        private string _languageCode = "en-US";

        // Method to get supported languages for transcription
        // Currently language codes are hardcoded
        private async Task<Dictionary<string, string>> GetSupportedLanguagesAsync()
        {
            var languages = new Dictionary<string, string>
        {
            { "en-US", "English (US)" },
            { "en-GB", "English (UK)" },
            { "en-AU", "English (AU)" },
            { "en-IN", "English (IN)" },
            { "es-US", "Spanish (US)" },
            { "es-ES", "Spanish (ES)" },
            { "fr-FR", "French (FR)" },
            { "fr-CA", "French (CA)" },
            { "de-DE", "German (DE)" },
            { "it-IT", "Italian (IT)" },
            { "pt-BR", "Portuguese (BR)" },
            { "pt-PT", "Portuguese (PT)" },
            { "nl-NL", "Dutch (NL)" },
            { "hi-IN", "Hindi (IN)" },
            { "ko-KR", "Korean (KR)" },
            { "ja-JP", "Japanese (JP)" },
            { "zh-CN", "Mandarin Chinese (Mainland)" },
            { "ar-SA", "Arabic (SA)" },
            { "ru-RU", "Russian (RU)" },
            { "tr-TR", "Turkish (TR)" }
        };


            return await Task.FromResult(languages);
        }

        // Method to populate language combo box
        private async void PopulateLanguageComboBox()
        {

            try
            {
                var languages = await GetSupportedLanguagesAsync();
                cbxLanguages.DataSource = new BindingSource(languages, null);
                cbxLanguages.DisplayMember = "Value";
                cbxLanguages.ValueMember = "Key";

                _languageCode = cbxLanguages.SelectedValue.ToString();
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching languages: {ex.Message}");
            }

            Debug.WriteLine(_languageCode);
        }

        // Constructor to initialize components
        public TranscribeApp()
        {
            InitializeComponent();

            // Event handler for the start transcription button
            btnStartTranscription.Click += async (sender, e) => await btnStartTranscription_Click();

            // Populate the language combo box
            PopulateLanguageComboBox();
        }

        // Method to update the rich text box with the current transcription text
        void updateGUI()
        {
            rtbTranscription.Text = currenttext;
            rtbTranscription.SelectionStart = rtbTranscription.TextLength;
            rtbTranscription.SelectionLength = 0;
            rtbTranscription.ScrollToCaret();
        }

        // Event handler for receiving transcription events
        private void TranscriptEvent(object? sender, TranscriptEvent transcriptEvent)
        {
            result = transcriptEvent.Transcript?.Results?.FirstOrDefault()?.Alternatives?.FirstOrDefault()?.Transcript;
            var ispartial = transcriptEvent.Transcript?.Results?.FirstOrDefault()?.IsPartial.ToString();

            
            if (result != null)
            {
                if (ispartial == "False")
                {

                    TranscribedText += result + "\n";
                    currenttext = TranscribedText;

                    //Debug.WriteLine(TranscribedText);

                    if (InvokeRequired)
                    {
                       Invoke(new MethodInvoker(updateGUI));
                    } else
                    {
                       updateGUI();
                    }
                }
                else
                {
                    currenttext = TranscribedText + "\n" + result;
                    //if (InvokeRequired)
                    //{
                    //   Invoke(new MethodInvoker(updateGUI));
                    //}
                    //else
                    //{
                    //    updateGUI();
                    //}

                    Debug.WriteLine("+");
                    return;
                }
            }
            //Console.WriteLine("TEST: " + result);
        }

        // Event handler for transcription exceptions
        private static void TranscribeException(object? sender, TranscribeException exception)
        {
            Debug.WriteLine("Error: " + exception.ExceptionType);
            Debug.WriteLine(exception.Message);
        }

        // Load event for the form
        private void TranscribeApp_Load(object sender, EventArgs e)
        {

        }
        
        // Event handler for start/stop transcription button click
        private async Task btnStartTranscription_Click()
        {

            if (btnStartTranscription.Text == "Stop Transcription")
            {
                
                // Stop the recording and streaming
                _waveIn.StopRecording();
                await client.StopStreaming();
                btnStartTranscription.Text = "Start Transcription";
                cbxLanguages.Enabled = true;
                return;

            }

            var region = "us-east-1";
            Config config = new Config("pcm", "8000", _languageCode);
            //Config config = new Config("pcm", "8000");
            config.EnableChannelIdentification = "true";
            config.NumberOfChannels = "2";
            config.EnablePartialResultsStabilization = "true";
            client = new AmazonTranscribeStreamingClient(region, config, null);

            client.TranscriptEvent += TranscriptEvent;
            client.TranscriptException += TranscribeException;

            btnStartTranscription.Text = "Stop Transcription";
            cbxLanguages.Enabled = false;
            await client.StartStreaming();

            int chunkSizeTime = 100; // 100 millisecond chunks
            int chunkSize = 8000 * 2 / (1000 / chunkSizeTime); // calculate the chunk size

            _waveIn = new WaveInEvent
            {
                WaveFormat = new WaveFormat(16000, 1), // 16kHz, mono
                DeviceNumber = 1
            };

            _memoryStream = new MemoryStream();
            _waveIn.DataAvailable += (s, a) => _memoryStream.Write(a.Buffer, 0, a.BytesRecorded);
            _waveIn.RecordingStopped += (s, a) => _memoryStream.Close();

            _waveIn.StartRecording();

            //TranscribedText += "\n Transcription started.\n";
            //TranscribedText += _languageCode + "\n";

            _waveIn.DataAvailable += async (s, a) =>
            {

                client.StreamBuffer(a.Buffer);

                await _memoryStream.FlushAsync();
            };


        }
        
        // Event handler for language combo box selection change
        private void cbxLanguages_SelectedIndexChanged(object sender, EventArgs e)
        {
            _languageCode = cbxLanguages.SelectedValue.ToString();
        }

    }
}
