// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
// SPDX-License-Identifier: Apache-2.0

namespace Amazon.TranscribeStreamingService {

  public class AudioEvent : ByteSerializable {
    private byte[] _audioData;

    public AudioEvent(byte[] audioData) {
      this._audioData = audioData;
    }

    public byte[] Serialize() {
      Message message = new Message();
      message.Payload = this._audioData;
      message.Headers = new Header[3];
      message.Headers[0] = new Header(":content-type", "application/octet-stream");
      message.Headers[1] = new Header(":event-type", "AudioEvent");
      message.Headers[2] = new Header(":message-type", "event");
      message.CalculatePrelude();

      MessageCrc msgAndCrc = new MessageCrc(message);

      return msgAndCrc.Serialize();
    }
  }


}
