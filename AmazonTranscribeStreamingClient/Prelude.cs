// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
// SPDX-License-Identifier: Apache-2.0

using System.Runtime.InteropServices;

namespace Amazon.TranscribeStreamingService;

[StructLayout(LayoutKind.Sequential)]
public struct Prelude : ByteSerializable {

  public UInt32 TotalByteLength;

  public UInt32 HeadersByteLength;

  public byte[] Serialize() {
    byte[] serialized = new byte[8];

    byte[] totalBytesLength = BitConverter.GetBytes(TotalByteLength);
    byte[] headerBytesLength = BitConverter.GetBytes(HeadersByteLength);
    if(BitConverter.IsLittleEndian) {
      Array.Reverse(totalBytesLength);
      Array.Reverse(headerBytesLength);
    }

    System.Buffer.BlockCopy(totalBytesLength, 0, serialized,0,4);
    System.Buffer.BlockCopy(headerBytesLength, 0, serialized,4,4);
    return serialized;
  }
}

