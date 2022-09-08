// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
// SPDX-License-Identifier: Apache-2.0

namespace Amazon.TranscribeStreamingService 
{
    public class AudioEventMessage
    {
        public UInt32 totallength {get; set;}
        public UInt32 headerslength {get; set;}
        public UInt32 crc {get; set;}
        public Dictionary<string, Dictionary<string, string>>?  headers { get; set;}
        public byte[]? body {get; set;} 
        public UInt32 msgcrc {get; set;}
    }
}
