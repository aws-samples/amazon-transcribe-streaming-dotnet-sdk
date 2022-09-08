// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
// SPDX-License-Identifier: Apache-2.0

namespace Amazon.TranscribeStreamingService {

  public struct MessageCrc : ByteSerializable {
    public Message Message;

    public UInt32 Crc;

    public MessageCrc(Message message) {
      this.Message = message;
      this.Message.CalculatePrelude();
      this.Crc = Crc32.Singleton.Get(message.Serialize());
    }

    public byte[] Serialize() {
      byte[] message = Message.Serialize();
      byte[] crc = BitConverter.GetBytes(Crc);

      byte[] arr = new byte[message.Length + crc.Length];
      if(BitConverter.IsLittleEndian) {
        Array.Reverse(crc);
      }

      // copy message
      System.Buffer.BlockCopy(message, 0, arr, 0, message.Length);
      System.Buffer.BlockCopy(crc, 0, arr,  message.Length, crc.Length);

      return arr;
    }
  }

}