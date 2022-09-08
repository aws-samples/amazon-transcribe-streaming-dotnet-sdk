// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
// SPDX-License-Identifier: Apache-2.0

namespace Amazon.TranscribeStreamingService.Models {

    public class TranscribeException {
        public string? ExceptionType { get; set;}
        public string? Message { get; set; }
    }

    public class Alternative
    {
        public List<Item>? Items { get; set; }
        public string? Transcript { get; set; }
    }

    public class Item
    {
        public string? Content { get; set; }
        public double EndTime { get; set; }
        public double StartTime { get; set; }
        public string? Type { get; set; }
        public bool VocabularyFilterMatch { get; set; }
    }

    public class Result
    {
        public List<Alternative>? Alternatives { get; set; }
        public double EndTime { get; set; }
        public bool IsPartial { get; set; }
        public string? ResultId { get; set; }
        public double StartTime { get; set; }
    }

    public class TranscriptEvent
    {
        public Transcript? Transcript { get; set; }
    }

    public class Transcript
    {
        public List<Result>? Results { get; set; }
    }



}