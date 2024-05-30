using System;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

using Amazon;
using Amazon.TranscribeService;
using Amazon.TranscribeStreamingService;
using Amazon.TranscribeStreamingService.Models;

using NAudio;
using NAudio.Wave;
using static System.Net.Mime.MediaTypeNames;




namespace Amazon.TranscribeStreamingService.Example
{
    public partial class TranscribeApp : Form
    {

        // Audio recording components
        private WaveInEvent? _microphoneCapture;
        private WasapiLoopbackCapture? _loopbackCapture;

        private BufferedWaveProvider _micBuffer;
        private BufferedWaveProvider _loopbackBuffer;
        private MixingWaveProvider32 _mixer;

        private MemoryStream? _memoryStream;
        private Task? _sendAudioTask;
        private Task? _receiveTranscriptionTask;

        // Recording state
        private bool _isRecording = false;

        // Text variables for holding transcriptions
        private static string TranscribedText = "";
        private static string result = "";

        // Amazon Transcribe Streaming Client
        private static AmazonTranscribeStreamingClient speakersClient;
        private static AmazonTranscribeStreamingClient clientMic;

        // Default language code
        private string _languageCode = "en-US";
        private string _speakersLanguageCode = "en-US";

        // selected Wave In Device
        private int _selectedWaveInDevice = 0;

        // selected Wave Out Device


        // Method to get supported languages for transcription
        // Currently language codes are hardcoded
        private async Task<Dictionary<string, string>> GetSupportedLanguagesAsync()
        {
            var languages = new Dictionary<string, string>
        {
            {"en-US", "English, US"},
            {"en-GB", "English, British"},
            {"en-AU", "English, Australian"},
            {"zh-CN", "Chinese, Simplified"},
            {"fr-FR", "French"},
            {"fr-CA", "French, Canadian"},
            {"de-DE", "German"},
            {"hi-IN", "Hindi, Indian"},
            {"it-IT", "Italian"},
            {"ja-JP", "Japanese"},
            {"ko-KR", "Korean"},
            {"pt-BR", "Portuguese, Brazilian"},
            {"es-US", "Spanish, US"},
            {"th-TH", "Thai"}

        };


            return await Task.FromResult(languages);
        }

        private void PopulateWaveInDevices(ComboBox comboBox)
        {
            comboBox.Items.Clear();

            for (int i = 0; i < WaveIn.DeviceCount; i++)
            {
                var deviceInfo = WaveIn.GetCapabilities(i);
                comboBox.Items.Add($"{i}: {deviceInfo.ProductName}");
            }

            if (comboBox.Items.Count > 0)
            {
                comboBox.SelectedIndex = 0; // Select the first device by default
                _selectedWaveInDevice = 0;
            }
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

                cbxSpkrLanguages.DataSource = new BindingSource(languages, null);
                cbxSpkrLanguages.DisplayMember = "Value";
                cbxSpkrLanguages.ValueMember = "Key";
                _speakersLanguageCode = cbxSpkrLanguages.SelectedValue.ToString();
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
            btnRecordMike.Click += async (sender, e) => await btnRecordMike_Click();
            btnRecordSpeakers.Click += async (sender, e) => await btnRecordSpeakers_Click();

            // Populate the language combo box
            PopulateLanguageComboBox();

            // Populate the wave in device combo box
            PopulateWaveInDevices(cbxInputDevices);
        }

        // Method to update the rich text box with the current transcription text
        void updateGUI(string txt, RichTextBox rtb)
        {
            // Append the new text to the existing text)
            rtb.Text = txt;

            rtb.SelectionStart = rtbTranscription.TextLength;
            rtb.SelectionLength = 0;
            rtb.ScrollToCaret();
        }

        // Event handler for receiving transcription events
        private void TranscriptEventMike(object? sender, TranscriptEvent transcriptEvent)
        {
            result = transcriptEvent.Transcript?.Results?.FirstOrDefault()?.Alternatives?.FirstOrDefault()?.Transcript;
            var ispartial = transcriptEvent.Transcript?.Results?.FirstOrDefault()?.IsPartial.ToString();



            if (result != null)
            {
                Debug.WriteLine(result.Length.ToString() + " - " + ispartial.ToString() + " - " + result);
                if (ispartial == "False")
                {

                    TranscribedText += result + "\n";


                    //Debug.WriteLine(TranscribedText);

                    //if (InvokeRequired)
                    //{
                    //   Invoke(new MethodInvoker(() => updateGUI(TranscribedText)));
                    //} else
                    //{
                    //   updateGUI(TranscribedText);
                    //}
                }
                else
                {
                    //string currenttext = TranscribedText + "\n" + result;


                    if (InvokeRequired)
                    {
                        Invoke(new MethodInvoker(() => updateGUI(result, rtbTranscriptionMike)));
                    }
                    else
                    {
                        updateGUI(result, rtbTranscriptionMike);
                    }

                    Debug.WriteLine("+");
                    return;
                }
            }

            //Console.WriteLine("TEST: " + result);
        }

        private void TranscriptSpeakersEvent(object? sender, TranscriptEvent transcriptEvent)
        {
            result = transcriptEvent.Transcript?.Results?.FirstOrDefault()?.Alternatives?.FirstOrDefault()?.Transcript;
            var ispartial = transcriptEvent.Transcript?.Results?.FirstOrDefault()?.IsPartial.ToString();

            if (result != null)
            {

                var starttime = transcriptEvent.Transcript.Results.FirstOrDefault().StartTime;
                var alternatives = transcriptEvent.Transcript?.Results?.FirstOrDefault().Alternatives.Count;

                Debug.WriteLine(result.Length.ToString() + " - " + ispartial.ToString() + " - " + starttime.ToString() + " - " + alternatives.ToString() + " - " + result);
                if (ispartial == "False")
                {

                    TranscribedText += result + "\n";


                    //Debug.WriteLine(TranscribedText);

                    //if (InvokeRequired)
                    //{
                    //   Invoke(new MethodInvoker(() => updateGUI(TranscribedText)));
                    //} else
                    //{
                    //   updateGUI(TranscribedText);
                    //}
                }
                else
                {
                    //string currenttext = TranscribedText + "\n" + result;


                    if (InvokeRequired)
                    {
                        Invoke(new MethodInvoker(() => updateGUI(result, rtbTranscription)));
                    }
                    else
                    {
                        updateGUI(result, rtbTranscription);
                    }

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


        private async Task btnRecordSpeakers_Click()
        {
            if (btnRecordSpeakers.Text == "Stop Speakers Transcription")
            {

                // Stop the recording and streaming
                _loopbackCapture.StopRecording();
                //_microphoneCapture.StopRecording();

                await speakersClient.StopStreaming();
                btnRecordSpeakers.Text = "Start Speakers Transcription";
                cbxSpkrLanguages.Enabled = true;

                return;

            }

            _loopbackCapture = new WasapiLoopbackCapture
            {
                WaveFormat = new WaveFormat(16000, 16, 2)
            };

            var waveFormat = _loopbackCapture.WaveFormat;
            int sampleRate = waveFormat.SampleRate;
            int channels = waveFormat.Channels;

            var region = "us-east-1";

            Config config = new Config("pcm", sampleRate.ToString(), _speakersLanguageCode);
            //config.Language = null;
            //config.IdentifyMultipleLanguages = "true";
            config.EnableChannelIdentification = "true";
            config.NumberOfChannels = "2";
            config.EnablePartialResultsStabilization = "true";

            speakersClient = new AmazonTranscribeStreamingClient(region, config, null);

            speakersClient.TranscriptEvent += TranscriptSpeakersEvent;
            speakersClient.TranscriptException += TranscribeException;

            btnRecordSpeakers.Text = "Stop Speakers Transcription";
            cbxSpkrLanguages.Enabled = false;
            await speakersClient.StartStreaming();

            int chunkSizeTime = 100; // 100 millisecond chunks
            int chunkSize = 8000 * 2 / (1000 / chunkSizeTime); // calculate the chunk size

            _memoryStream = new MemoryStream();

            _loopbackCapture.DataAvailable += (s, a) => _memoryStream.Write(a.Buffer, 0, a.BytesRecorded);

            _loopbackCapture.RecordingStopped += (s, a) => _memoryStream.Close();

            _loopbackCapture.StartRecording();

            _loopbackCapture.DataAvailable += async (s, a) =>
            {
                speakersClient.StreamBuffer(a.Buffer);
                await _memoryStream.FlushAsync();
            };



        }


        // Event handler for start/stop transcription button click
        private async Task btnRecordMike_Click()
        {

            if (btnRecordMike.Text == "Stop Mike Transcription")
            {

                // Stop the recording and streaming
                _microphoneCapture.StopRecording();
                await clientMic.StopStreaming();
                btnRecordMike.Text = "Start Mike Transcription";
                cbxLanguages.Enabled = true;
                return;

            }

            var region = "us-east-1";
            Config config = new Config("pcm", "8000", _languageCode);
            //Config config = new Config("pcm", "8000");
            config.EnableChannelIdentification = "true";
            config.NumberOfChannels = "2";
            config.EnablePartialResultsStabilization = "true";

            clientMic = new AmazonTranscribeStreamingClient(region, config, null);

            clientMic.TranscriptEvent += TranscriptEventMike;
            clientMic.TranscriptException += TranscribeException;

            btnRecordMike.Text = "Stop Mike Transcription";
            cbxLanguages.Enabled = false;
            await clientMic.StartStreaming();

            int chunkSizeTime = 100; // 100 millisecond chunks
            int chunkSize = 8000 * 2 / (1000 / chunkSizeTime); // calculate the chunk size

            _microphoneCapture = new WaveInEvent
            {
                WaveFormat = new WaveFormat(16000, 1), // 16kHz, mono
                DeviceNumber = _selectedWaveInDevice
            };

            _memoryStream = new MemoryStream();
            _microphoneCapture.DataAvailable += (s, a) => _memoryStream.Write(a.Buffer, 0, a.BytesRecorded);
            _microphoneCapture.RecordingStopped += (s, a) => _memoryStream.Close();

            _microphoneCapture.StartRecording();

            //TranscribedText += "\n Transcription started.\n";
            //TranscribedText += _languageCode + "\n";

            _microphoneCapture.DataAvailable += async (s, a) =>
            {

                clientMic.StreamBuffer(a.Buffer);

                await _memoryStream.FlushAsync();
            };


        }

        // Event handler for language combo box selection change
        private void cbxLanguages_SelectedIndexChanged(object sender, EventArgs e)
        {
            _languageCode = cbxLanguages.SelectedValue.ToString();
        }


        private void cbxSpkrLanguages_SelectedIndexChanged(object sender, EventArgs e)
        {
            _speakersLanguageCode = cbxSpkrLanguages.SelectedValue.ToString();
        }


    }
}
