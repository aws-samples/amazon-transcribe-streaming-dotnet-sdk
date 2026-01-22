# Amazon Transcribe Streaming .NET SDK

A comprehensive .NET 6.0 client library for Amazon Transcribe Streaming that enables real-time speech-to-text transcription through WebSocket connections.

## Overview

This SDK provides an implementation of the Amazon Transcribe Streaming API, handling the complexities of WebSocket communication, AWS Signature Version 4 authentication, and event stream protocol serialization. It supports both single-language and multi-language transcription with additional configuration options.

## Features

### Core Capabilities
- **Real-time Streaming**: Bidirectional WebSocket communication for live audio transcription
- **Flexible Audio Input**: Stream from files (simulated real-time) or live audio buffers
- **Multi-Language Support**: Automatic language identification across multiple languages in a single stream
- **Channel Identification**: Transcribe multi-channel audio with per-channel output
- **Custom Vocabularies**: Support for custom vocabulary and vocabulary filters
- **Content Redaction**: Automatic PII (Personally Identifiable Information) redaction or identification
- **Speaker Diarization**: Identify and label different speakers in the audio
- **Partial Results Stabilization**: Reduce latency with configurable stability levels

### Advanced Features
- **ConfigurationEvent Support**: Configure channel definitions and post-call analytics settings
- **Enhanced Response Models**: Access confidence scores, speaker labels, entity detection, and language identification scores
- **Automatic Reconnection**: Built-in reconnection handling for network interruptions
- **Event-Driven Architecture**: Subscribe to transcript and exception events for clean integration

## Installation

### Prerequisites
- .NET 6.0 or later
- AWS credentials with Amazon Transcribe permissions

### Dependencies
```xml
<PackageReference Include="AWSSDK.Core" Version="3.7.12.18" />
<PackageReference Include="Websocket.Client" Version="4.4.43" />
```

## Quick Start

### Basic Usage

```csharp
using Amazon.TranscribeStreamingService;
using Amazon.TranscribeStreamingService.Models;
using Amazon.Runtime;

// Configure transcription parameters
Config config = new Config("pcm", "8000", "en-US");

// Set up AWS credentials
var credentials = new SessionAWSCredentials(accessKey, secretKey, sessionToken);

// Create client
var client = new AmazonTranscribeStreamingClient("us-east-1", config, credentials);

// Subscribe to events
client.TranscriptEvent += (sender, transcriptEvent) => {
    var transcript = transcriptEvent.Transcript?.Results?.FirstOrDefault()
        ?.Alternatives?.FirstOrDefault()?.Transcript;
    Console.WriteLine(transcript);
};

client.TranscriptException += (sender, exception) => {
    Console.WriteLine($"Error: {exception.ExceptionType} - {exception.Message}");
};

// Start streaming
await client.StartStreaming();

// Stream audio file
client.StreamFile("audio.wav", chunkSize: 1600, sleepTimeMs: 100);

// Or stream live audio buffers
client.StreamBuffer(audioByteArray);
```

### Multi-Language Identification

```csharp
// Configure for automatic multi-language detection
Config config = new Config("pcm", "8000");
config.IdentifyMultipleLanguages = "true";
config.LanguageOptions = "en-US,es-US,fr-FR";
config.PreferredLanguage = "en-US"; // Optional hint

var client = new AmazonTranscribeStreamingClient("us-east-1", config, credentials);

client.TranscriptEvent += (sender, transcriptEvent) => {
    var result = transcriptEvent.Transcript?.Results?.FirstOrDefault();
    
    // Access detected language
    Console.WriteLine($"Language: {result?.LanguageCode}");
    
    // Access language identification scores
    foreach (var lang in result?.LanguageIdentification ?? Enumerable.Empty<LanguageWithScore>()) {
        Console.WriteLine($"{lang.LanguageCode}: {lang.Score:F2}");
    }
};

await client.StartStreaming();
```

### Channel Identification (Stereo Audio)

```csharp
Config config = new Config("pcm", "8000", "en-US");
config.EnableChannelIdentification = "true";
config.NumberOfChannels = "2";

var client = new AmazonTranscribeStreamingClient("us-east-1", config, credentials);

client.TranscriptEvent += (sender, transcriptEvent) => {
    var result = transcriptEvent.Transcript?.Results?.FirstOrDefault();
    Console.WriteLine($"Channel {result?.ChannelId}: {result?.Alternatives?.FirstOrDefault()?.Transcript}");
};

await client.StartStreaming();
```

### ConfigurationEvent for Post-Call Analytics

```csharp
var configEvent = new ConfigurationEvent {
    ChannelDefinitions = new List<ChannelDefinition> {
        new ChannelDefinition { ChannelId = 0, ParticipantRole = "AGENT" },
        new ChannelDefinition { ChannelId = 1, ParticipantRole = "CUSTOMER" }
    },
    PostCallAnalyticsSettings = new PostCallAnalyticsSettings {
        OutputLocation = "s3://my-bucket/analytics/",
        DataAccessRoleArn = "arn:aws:iam::123456789012:role/TranscribeRole"
    }
};

await client.StartStreaming(configEvent);
```

## Configuration Options

### Required Parameters
- `MediaEncoding`: Audio encoding format (`pcm`, `ogg-opus`, `flac`)
- `SampleRate`: Audio sample rate in Hz (8000-48000)
- `Language` OR `IdentifyLanguage` OR `IdentifyMultipleLanguages`: Language configuration

### Optional Parameters

#### Language Settings
- `LanguageOptions`: Comma-separated language codes for automatic identification
- `PreferredLanguage`: Hint for most likely language
- `LanguageModelName`: Custom language model name

#### Vocabulary
- `VocabularyName`: Custom vocabulary for single language
- `VocabularyNames`: Custom vocabularies for multi-language
- `VocabularyFilterName`: Vocabulary filter for single language
- `VocabularyFilterNames`: Vocabulary filters for multi-language
- `VocabularyFilterMethod`: Filter method (`remove`, `mask`, `tag`)

#### Content Processing
- `ContentIdentificationType`: Identify PII (`PII`)
- `ContentRedactionType`: Redact PII (`PII`)
- `PiiEntityTypes`: Specific PII types to process

#### Audio Processing
- `EnableChannelIdentification`: Enable multi-channel transcription
- `NumberOfChannels`: Number of audio channels (must be 2)
- `ShowSpeakerLabel`: Enable speaker diarization

#### Result Optimization
- `EnablePartialResultsStabilization`: Reduce latency with stable partial results
- `PartialResultsStability`: Stability level (`high`, `medium`, `low`)

#### Session Management
- `SessionId`: Custom session identifier (UUID format)

## Response Models

### TranscriptEvent
Contains the transcription results with the following structure:

```csharp
public class TranscriptEvent {
    public Transcript Transcript { get; set; }
}

public class Transcript {
    public List<Result> Results { get; set; }
}

public class Result {
    public List<Alternative> Alternatives { get; set; }
    public string ResultId { get; set; }
    public double StartTime { get; set; }
    public double EndTime { get; set; }
    public bool IsPartial { get; set; }
    public string ChannelId { get; set; }           // NEW
    public string LanguageCode { get; set; }        // NEW
    public List<LanguageWithScore> LanguageIdentification { get; set; }  // NEW
}

public class Alternative {
    public string Transcript { get; set; }
    public List<Item> Items { get; set; }
    public List<Entity> Entities { get; set; }     // NEW
}

public class Item {
    public string Content { get; set; }
    public string Type { get; set; }
    public double StartTime { get; set; }
    public double EndTime { get; set; }
    public bool VocabularyFilterMatch { get; set; }
    public double? Confidence { get; set; }         // NEW
    public string Speaker { get; set; }             // NEW
    public bool? Stable { get; set; }               // NEW
}

public class Entity {                               // NEW
    public string Category { get; set; }
    public double? Confidence { get; set; }
    public string Content { get; set; }
    public string Type { get; set; }
    public double StartTime { get; set; }
    public double EndTime { get; set; }
}

public class LanguageWithScore {                    // NEW
    public string LanguageCode { get; set; }
    public double Score { get; set; }
}
```

## Examples

The repository includes two example applications:

### StreamFile
Basic example demonstrating single-language transcription from an audio file.

```bash
cd Examples/StreamFile
dotnet run
```

### StreamFileMultiLanguage
Advanced example demonstrating multi-language identification with enhanced output.

```bash
cd Examples/StreamFileMultiLanguage
dotnet run
```

Both examples require AWS credentials set as environment variables:
```bash
export AWS_ACCESS_KEY_ID="your-access-key"
export AWS_SECRET_ACCESS_KEY="your-secret-key"
export AWS_SESSION_TOKEN="your-session-token"  # if using temporary credentials
export AWS_DEFAULT_REGION="us-east-1"
```

## Architecture

### Authentication
The SDK implements AWS Signature Version 4 signing for WebSocket connections, generating presigned URLs with proper authentication headers. It supports both long-term credentials and temporary session tokens (IAM roles).

### Event Stream Protocol
Audio and configuration data are transmitted using AWS event stream encoding with:
- Binary message framing with prelude and headers
- CRC32 checksums for message integrity
- Support for AudioEvent and ConfigurationEvent message types

### Connection Management
Built on the Websocket.Client library with automatic reconnection support and configurable timeout settings.

## What's New

### Latest Release

#### Multi-Language Support
- Added `IdentifyMultipleLanguages` configuration parameter for automatic language detection across multiple languages in a single stream
- New `LanguageWithScore` model provides confidence scores for each detected language
- Enhanced `Result` model includes `LanguageCode` and `LanguageIdentification` fields

#### ConfigurationEvent Support
- Implemented `ConfigurationEvent` for advanced stream configuration
- Support for `ChannelDefinitions` to map channels to participant roles (AGENT/CUSTOMER)
- Support for `PostCallAnalyticsSettings` to enable post-call analytics with S3 output and KMS encryption
- New `ConfigurationEventMessage` class for event serialization

#### Enhanced Response Models
- **Item enhancements**: Added `Confidence` (transcription confidence score), `Speaker` (speaker label), and `Stable` (partial result stability flag)
- **Alternative enhancements**: Added `Entities` array for PII entity detection with category, confidence, content, and timing information
- **Result enhancements**: Added `ChannelId` for multi-channel identification and language detection fields

#### API Completeness
- Full alignment with Amazon Transcribe Streaming API specification for `StartStreamTranscription`
- Support for all documented request parameters and response fields
- Backward compatible with existing implementations

## Supported Audio Formats

- **PCM**: 16-bit signed little-endian (does not include WAV headers)
- **Opus**: Opus-encoded audio in Ogg container
- **FLAC**: Free Lossless Audio Codec

Sample rates: 8000 Hz to 48000 Hz

## Error Handling

The SDK provides structured exception handling through the `TranscriptException` event:

```csharp
client.TranscriptException += (sender, exception) => {
    switch (exception.ExceptionType) {
        case "BadRequestException":
            // Invalid parameters
            break;
        case "LimitExceededException":
            // Audio length limit exceeded
            break;
        case "ConflictException":
            // Session ID conflict
            break;
        case "InternalFailureException":
            // Service error
            break;
        case "ServiceUnavailableException":
            // Service temporarily unavailable
            break;
    }
};
```

## Best Practices

1. **Chunk Size**: Use 100-200ms audio chunks for optimal performance
2. **Sample Rate**: Match the sample rate to your audio source quality (8kHz for telephony, 16kHz+ for high-quality audio)
3. **Language Options**: Limit language options to 5 or fewer for best accuracy in multi-language mode
4. **Partial Results**: Enable partial results stabilization for lower latency in real-time applications
5. **Error Handling**: Always subscribe to `TranscriptException` events for production deployments
6. **Connection Management**: Monitor `IsConnected` property and implement reconnection logic for long-running streams

## Limitations

- Maximum audio stream length: Varies by service limits
- Multi-channel support: Limited to 2 channels (stereo)
- Language identification: Cannot include multiple dialects of the same language (e.g., en-US and en-GB)
- Custom language models: Cannot be combined with automatic language identification

## Contributing

See [CONTRIBUTING.md](CONTRIBUTING.md) for guidelines on contributing to this project.

## License

This library is licensed under the Apache 2.0 License. See the [LICENSE](LICENSE) file for details.

## Support

For issues and feature requests, please use the GitHub issue tracker.

For Amazon Transcribe service documentation, visit the [AWS Documentation](https://docs.aws.amazon.com/transcribe/latest/dg/streaming.html).

