// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
// SPDX-License-Identifier: Apache-2.0

namespace Amazon.TranscribeStreamingService {
    public class TranscriptEventArgs : EventArgs
    {
        public string? Transcript { get; set; }
    }
}