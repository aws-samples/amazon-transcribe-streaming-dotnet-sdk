// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
// SPDX-License-Identifier: Apache-2.0

using System.Text.Json;

namespace Amazon.TranscribeStreamingService {

  public class ConfigurationEventMessage : ByteSerializable {
    private byte[] _configData;

    public ConfigurationEventMessage(Models.ConfigurationEvent configEvent) {
      this._configData = JsonSerializer.SerializeToUtf8Bytes(configEvent);
    }

    public byte[] Serialize() {
      Message message = new Message();
      message.Payload = this._configData;
      message.Headers = new Header[3];
      message.Headers[0] = new Header(":content-type", "application/json");
      message.Headers[1] = new Header(":event-type", "ConfigurationEvent");
      message.Headers[2] = new Header(":message-type", "event");
      message.CalculatePrelude();

      MessageCrc msgAndCrc = new MessageCrc(message);

      return msgAndCrc.Serialize();
    }
  }
}
