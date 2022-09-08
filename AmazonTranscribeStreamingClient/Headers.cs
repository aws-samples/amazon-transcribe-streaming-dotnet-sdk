// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
// SPDX-License-Identifier: Apache-2.0

using System.Runtime.InteropServices;
using System.Text;

namespace Amazon.TranscribeStreamingService {

  public struct Header : ByteSerializable {
    public byte NameLength;
    public byte[] Name;
    public byte ValueType;
    public UInt16 ValueLength;
    public byte[] Value;

    public Header(string Name, string Value) {
      UTF8Encoding utf8 = new UTF8Encoding();
      this.Name = utf8.GetBytes(Name);
      this.Value = utf8.GetBytes(Value);
      this.ValueType = 7;
      this.NameLength = (byte)Name.Length;
      this.ValueLength = Convert.ToUInt16(Value.Length);
    }

    public UInt32 GetLength() {
      return Convert.ToUInt32(4 + Name.Length + Value.Length);
    }

    public byte[] Serialize() {
      byte[] arr = new byte[1 + Name.Length + 1 + 2 + Value.Length];
      int pos = 0;
      // copy name length
      System.Buffer.BlockCopy(BitConverter.GetBytes(NameLength), 0, arr, pos,1);
      pos += 1;
      
      // copy name
      System.Buffer.BlockCopy(Name, 0, arr, pos, Name.Length);
      pos += Name.Length;
      
      // copy value type
      System.Buffer.BlockCopy(BitConverter.GetBytes(ValueType), 0, arr, pos, 1);
      pos += 1;

      // copy value length
      byte[] valueLength = BitConverter.GetBytes(ValueLength);
      if(BitConverter.IsLittleEndian) { 
        Array.Reverse(valueLength);
      }

      System.Buffer.BlockCopy(valueLength, 0, arr, pos, 2);
      pos += 2;

      // copy value 
      System.Buffer.BlockCopy(Value, 0, arr, pos, Value.Length);
      pos += Value.Length;

      return arr;
    }

  }
}