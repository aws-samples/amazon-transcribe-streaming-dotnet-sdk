// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
// SPDX-License-Identifier: Apache-2.0

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.TranscribeStreamingService;
using Amazon.TranscribeStreamingService.Models;
using Amazon.Runtime;

namespace Amazon.TranscribeStreamingService.MultiLanguageExample
{    
    public class Program
    {        
        private static void TranscriptEvent(object? sender, TranscriptEvent transcriptEvent) {
          var result = transcriptEvent.Transcript?.Results?.FirstOrDefault();
          if (result != null) {
            var alternative = result.Alternatives?.FirstOrDefault();
            
            // Display detected language if available
            if (result.LanguageCode != null) {
              Console.Write($"[{result.LanguageCode}] ");
            }
            
            // Display language identification scores if available
            if (result.LanguageIdentification != null && result.LanguageIdentification.Any()) {
              Console.Write("Languages: ");
              foreach (var lang in result.LanguageIdentification) {
                Console.Write($"{lang.LanguageCode}({lang.Score:F2}) ");
              }
              Console.Write("- ");
            }
            
            Console.WriteLine(alternative?.Transcript);
          }
        }

        private static void TranscribeException(object? sender, TranscribeException exception) {
          Console.WriteLine("Error: " + exception.ExceptionType );
          Console.WriteLine(exception.Message);
        }

        static async Task Main(string[] args)
        {
          string? fileToStream = "../StreamFile/Auto2_GUID_002_AGENT_SteveE_DT_2022-03-20T08-02-20-mono.wav";

          // Configure for multi-language identification
          Config config = new Config("pcm", "8000");
          config.IdentifyMultipleLanguages = "true";
          config.LanguageOptions = "en-US,es-US,fr-FR"; // Specify possible languages
          config.PreferredLanguage = "en-US"; // Optional: hint at most likely language

          //If the fileToStream is stereo, set the following parameters
          // config.EnableChannelIdentification = "true";
          // config.NumberOfChannels = "2";

          string? accessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
          string? secretKey = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");
          string? sessionToken = Environment.GetEnvironmentVariable("AWS_SESSION_TOKEN");
          string? region = Environment.GetEnvironmentVariable("AWS_DEFAULT_REGION");
          if (region == null) region = "us-east-1";
          SessionAWSCredentials basicCreds = new SessionAWSCredentials(accessKey, secretKey, sessionToken);

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
