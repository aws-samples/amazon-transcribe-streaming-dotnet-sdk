// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
// SPDX-License-Identifier: Apache-2.0

using System;
using System.Runtime.InteropServices;
using Amazon.TranscribeStreamingService;

namespace Amazon.TranscribeStreamingService {

  public struct Message : ByteSerializable {
    public Prelude Prelude;
    
    public UInt32 PreludeCrc;

    public Header[] Headers;

    public byte[] Payload;

    public void CalculatePrelude() {
      // calculate header length
      this.Prelude.HeadersByteLength = 0;
      foreach (var header in this.Headers)
      {
          this.Prelude.HeadersByteLength += header.GetLength();
      }
      this.Prelude.TotalByteLength = Convert.ToUInt32(12 + this.Prelude.HeadersByteLength + this.Payload.Length + 4);
      this.PreludeCrc = Crc32.Singleton.Get(this.Prelude.Serialize());
      if(BitConverter.IsLittleEndian) {
        this.PreludeCrc = Utils.SwapBytes(this.PreludeCrc);
      }
    }

    private byte[] SerializeHeaders() {
      int headerSize = 0;
      byte[][] serializedHeaders = new byte[Headers.Length][];
      for(int i = 0; i < Headers.Length; i++) {
        serializedHeaders[i] = Headers[i].Serialize();
        headerSize += serializedHeaders[i].Length;
      }

      byte[] arr = new byte[headerSize];
      int pos = 0;
      for(int i = 0; i < Headers.Length; i++) {
        System.Buffer.BlockCopy(serializedHeaders[i], 0, arr, pos,serializedHeaders[i].Length);
        pos += serializedHeaders[i].Length;
      }

      return arr;
    }

    public byte[] Serialize() {
      CalculatePrelude();
      byte[] prelude = Prelude.Serialize();
      byte[] preludeCrc = BitConverter.GetBytes(PreludeCrc);
      byte[] headers = this.SerializeHeaders();

      byte[] arr = new byte[prelude.Length + preludeCrc.Length + headers.Length + Payload.Length];
      int pos = 0;

      // copy prelude
      System.Buffer.BlockCopy(prelude, 0, arr, pos, prelude.Length);
      pos += prelude.Length;

      // copy preludecrc
      System.Buffer.BlockCopy(preludeCrc, 0, arr, pos, preludeCrc.Length);
      pos += preludeCrc.Length;

      // copy headers
      System.Buffer.BlockCopy(headers, 0, arr, pos, headers.Length);
      pos += headers.Length;

      // copy headers
      System.Buffer.BlockCopy(Payload, 0, arr, pos, Payload.Length);
      pos += Payload.Length;

      return arr;
    }
  }
}
