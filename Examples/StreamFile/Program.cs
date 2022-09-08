// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
// SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Websocket.Client;
using Amazon.TranscribeStreamingService;
using Amazon.TranscribeStreamingService.Models;
using Amazon.Runtime;

// websocket example based on https://github.com/paulbatum/WebSocket-Samples/blob/master/HttpListenerWebSocketEcho/Client/Client.cs

namespace Amazon.TranscribeStreamingService.Example
{    
    public class Program
    {        
        private static void TranscriptEvent(object? sender, TranscriptEvent transcriptEvent) {
          Console.WriteLine(transcriptEvent.Transcript?.Results?.FirstOrDefault()?.Alternatives?.FirstOrDefault()?.Transcript);
        }

        private static void TranscribeException(object? sender, TranscribeException exception) {
          Console.WriteLine("Error: " + exception.ExceptionType );
          Console.WriteLine(exception.Message);
        }

        static async Task Main(string[] args)
        {

        //   string? fileToStream = "Auto2_GUID_002_AGENT_SteveE_DT_2022-03-20T08-02-20-mono.wav";
          string? fileToStream = "Auto2_GUID_002_AGENT_SteveE_DT_2022-03-20T08-02-20.wav";


          Config config = new Config("en-US", "pcm", "8000");
          //If the fileToStream is stereo, set the following parameters
        //   config.EnableChannelIdentification = "true";
        //   config.NumberOfChannels = "2";

          string? accessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
          string? secretKey = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");
          string? region = Environment.GetEnvironmentVariable("REGION");
          if (region == null) region = "us-east-1";
          BasicAWSCredentials basicCreds = new BasicAWSCredentials(accessKey, secretKey);

          AmazonTranscribeStreamingClient client = new AmazonTranscribeStreamingClient(region, config, basicCreds);
          client.TranscriptEvent += TranscriptEvent;
          client.TranscriptException += TranscribeException;
          await client.StartStreaming();

          int chunkSizeTime = 100; // 100 millisecond chunks
          int chunkSize = 8000 * 2 / (1000 / chunkSizeTime); // calculate the chunk size

          client.StreamFile(fileToStream, chunkSize, chunkSizeTime);
          while(client.IsConnected) {
            Thread.Sleep(1000);
          }
        }
    }
}