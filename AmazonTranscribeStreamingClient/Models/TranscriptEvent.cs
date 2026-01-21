// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
// SPDX-License-Identifier: Apache-2.0

namespace Amazon.TranscribeStreamingService.Models {

    public class TranscribeException {
        public string? ExceptionType { get; set;}
        public string? Message { get; set; }
    }

    public class Entity
    {
        public string? Category { get; set; }
        public double? Confidence { get; set; }
        public string? Content { get; set; }
        public double EndTime { get; set; }
        public double StartTime { get; set; }
        public string? Type { get; set; }
    }

    public class Alternative
    {
        public List<Item>? Items { get; set; }
        public string? Transcript { get; set; }
        public List<Entity>? Entities { get; set; }
    }

    public class Item
    {
        public string? Content { get; set; }
        public double EndTime { get; set; }
        public double StartTime { get; set; }
        public string? Type { get; set; }
        public bool VocabularyFilterMatch { get; set; }
        public double? Confidence { get; set; }
        public string? Speaker { get; set; }
        public bool? Stable { get; set; }
    }

    public class LanguageWithScore
    {
        public string? LanguageCode { get; set; }
        public double Score { get; set; }
    }

    public class Result
    {
        public List<Alternative>? Alternatives { get; set; }
        public double EndTime { get; set; }
        public bool IsPartial { get; set; }
        public string? ResultId { get; set; }
        public double StartTime { get; set; }
        public string? ChannelId { get; set; }
        public string? LanguageCode { get; set; }
        public List<LanguageWithScore>? LanguageIdentification { get; set; }
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