// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
// SPDX-License-Identifier: Apache-2.0

namespace Amazon.TranscribeStreamingService { 

    public class StaticAudioFile {
    
        public static List<byte[]> GetAudioStreamChunks(string filename, int chunkSize)
        {
            List<byte[]> bytes = new List<byte[]>();
            // string filename = "sample2.wav";
            FileStream SourceStream = File.Open(filename, FileMode.Open);
            var br = new BinaryReader(SourceStream);
            while(br.BaseStream.Position != br.BaseStream.Length)
            {
                byte[] chunk = new byte[chunkSize];
                chunk = br.ReadBytes(chunkSize);
                bytes.Add(chunk);
            }
            return bytes;
        }
    }
}