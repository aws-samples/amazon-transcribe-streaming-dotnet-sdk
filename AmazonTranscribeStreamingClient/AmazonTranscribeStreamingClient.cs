// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
// SPDX-License-Identifier: Apache-2.0

using Amazon.Runtime;
using Amazon.Runtime.EventStreams;
using Amazon.TranscribeStreamingService.Models;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Websocket.Client;


namespace Amazon.TranscribeStreamingService
{

  public class AmazonTranscribeStreamingClient
  {

    public event EventHandler<TranscriptEvent>? TranscriptEvent;
    public event EventHandler<TranscribeException>? TranscriptException;


    private readonly string _region;
    private readonly Config _config;
    private readonly AWSCredentials _credentials;
    private WebsocketClient? _client;
    
    public bool IsConnected { 
      get {
        if(_client != null) {
          return _client.IsRunning;
        } 
        else return false;
      } 
    }

    /// <summary>
    /// Instantiate a new streaming client.
    /// </summary>
    /// <param name="region">Which region to connect to, for example, us-west-2</param>
    /// <param name="config">Configuration parameters for the streaming session</param>
    /// <param name="credentials">AWS Credentials</param>
    public AmazonTranscribeStreamingClient(string region, Config config, AWSCredentials? credentials = null)
    {
      this._region = region;
      this._config = config;
      if (credentials == null) this._credentials = FallbackCredentialsFactory.GetCredentials();
      else this._credentials = credentials;
    }

    /// <summary>
    /// This opens a new websocket session and returns once it is connected.
    /// </summary>
    /// <returns>Nothing.</returns>
    public async Task StartStreaming()
    {
      TranscribePresignedUrl presigned = new TranscribePresignedUrl(this._region, this._config, this._credentials);
      string webSocketUrl = presigned.GetPresignedUrl();
      Debug.WriteLine(webSocketUrl);
      var url = new Uri(webSocketUrl);

      _client = new WebsocketClient(url);

      _client.ReconnectTimeout = TimeSpan.FromSeconds(30);
      _client.ReconnectionHappened.Subscribe(info =>
          Debug.WriteLine($"Reconnection happened, type: {info.Type}"));

      _client.DisconnectionHappened.Subscribe(info =>
          Debug.WriteLine($"Disconnect happened, type: {info.Type}"));

      _client.MessageReceived.Subscribe(msg =>
      {
        EventStreamMessage message = EventStreamMessage.FromBuffer(msg.Binary, 0, msg.Binary.Length);
        if(message.Headers.ContainsKey(":exception-type")){
          TranscribeException exception = new TranscribeException() {
            ExceptionType = message.Headers[":exception-type"].AsString(),
            Message =  System.Text.Encoding.UTF8.GetString(message.Payload)
          };
          this.TranscriptException?.Invoke(this, exception);
        }
        else if(message.Headers.ContainsKey(":event-type")) {
          string json = System.Text.Encoding.UTF8.GetString(message.Payload);
          TranscriptEvent? transcriptEvent = JsonSerializer.Deserialize<TranscriptEvent>(json);
          if(transcriptEvent != null) this.TranscriptEvent?.Invoke(this, transcriptEvent);
        }
      });
      await _client.Start();
    }

    /// <summary>
    /// If the websocket is open, this will terminate the connection.
    /// </summary>
    /// <returns>Nothing.</returns>
    public async Task StopStreaming()
    {
      if (this._client != null)
      {
        await _client.Stop(System.Net.WebSockets.WebSocketCloseStatus.NormalClosure, "Closing");
      }
    }

    /// <summary>
    /// This is a helper function designed to open an audio file and stream it, as if it was real
    /// time, to Amazon Transcribe.  This will help you consistently test streaming.
    /// </summary>
    /// <param name="filename">A path to the audio file to stream.</param>
    /// <param name="chunkSize">The size, in bytes, of the chunks to send. We suggest 100-200ms chunks.</param>
    /// <param name="sleepTimeMs">The amount of time to slepe between chunks to simulate real-time. 
    /// Transcribe Streaming will queue a few seconds of audio if you send ahead, but will error if the forward
    /// buffer it too large.
    /// </param>
    /// <exception cref="Exception">This will throw a generic exception if the client is not connected.</exception>
    public void StreamFile(string filename, int chunkSize, int sleepTimeMs)
    {
      if (this._client?.IsRunning == true)
      {
        List<byte[]> chunks = StaticAudioFile.GetAudioStreamChunks(filename, chunkSize);
        int currentChunk = 0;

        while (currentChunk < chunks.Count)
        {
          AudioEvent audioEvent = new AudioEvent(chunks[currentChunk]);
          byte[] buffer = audioEvent.Serialize();
          currentChunk += 1;
          // Debug.WriteLine("Sending " + currentChunk);
          _client.Send(buffer);
          Thread.Sleep(sleepTimeMs);
        }
      }
      else
      {
        throw new Exception("Client not connected.");
      }
    }

    /// <summary>
    /// This will stream a byte array 'chunk' to Transcribe. This is usually 100-200ms worth of audio.
    /// </summary>
    /// <param name="buffer">Byte array of the audio data chunk to send</param>
    /// <exception cref="Exception">This will throw an exception if the client is not connected.</exception>
    public void StreamBuffer(byte[] buffer)
    {
      if (this._client?.IsRunning == true)
      {
        AudioEvent audioEvent = new AudioEvent(buffer);
        byte[] eventBuffer = audioEvent.Serialize();
        _client.Send(buffer);
      }
      else
      {
        throw new Exception("Client not connected.");
      }
    }
  }
}