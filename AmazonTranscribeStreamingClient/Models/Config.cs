// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
// SPDX-License-Identifier: Apache-2.0

using System.Security.Principal;

namespace Amazon.TranscribeStreamingService.Models {
    public class Config {
        public string? Language;
        public string MediaEncoding;
        public string SampleRate;
        public string? VocabularyName { get; set; }
        public string? SessionId { get; set; }
        public string? VocabularyFilterName { get; set; }
        public string? VocabularyFilterMethod { get; set; }
        public string? ShowSpeakerLabel { get; set; }
        public string? EnableChannelIdentification { get; set; }
        public string? NumberOfChannels { get; set; }
        public string? EnablePartialResultsStabilization { get; set; }
        public string? PartialResultsStability { get; set; }
        public string? ContentIdentificationType { get; set; }
        public string? ContentRedactionType { get; set; }
        public string? PiiEntityTypes { get; set; }
        public string? LanguageModelName { get; set; }
        public string? IdentifyLanguage { get; set; }
        public string? IdentifyMultipleLanguages { get; set; }
        public string? LanguageOptions { get; set; }
        public string? PreferredLanguage { get; set; }
        public string? VocabularyNames { get; set; }
        public string? VocabularyFilterNames { get; set; }

        public Config(string mediaEncoding, string sampleRate) {
            this.MediaEncoding = mediaEncoding;
            this.SampleRate = sampleRate;
        }

        public Config(string mediaEncoding, string sampleRate, string language) {
            this.Language = language;
            this.MediaEncoding = mediaEncoding;
            this.SampleRate = sampleRate;
        }

        public SortedDictionary<string, string> GetDictionary() {
            SortedDictionary<string, string> dict = new SortedDictionary<string, string>();
            if(!string.IsNullOrEmpty(this.Language)) dict.Add("language-code",this.Language);
            dict.Add("media-encoding",this.MediaEncoding);
            dict.Add("sample-rate",this.SampleRate);
            if(!string.IsNullOrEmpty(this.VocabularyName)) dict.Add("vocabulary-name", this.VocabularyName);
            if(!string.IsNullOrEmpty(this.SessionId)) dict.Add("session-id", this.SessionId);
            if(!string.IsNullOrEmpty(this.VocabularyFilterName)) dict.Add("vocabulary-filter-name", this.VocabularyFilterName);
            if(!string.IsNullOrEmpty(this.VocabularyFilterMethod)) dict.Add("vocabulary-filter-method", this.VocabularyFilterMethod);
            if(!string.IsNullOrEmpty(this.ShowSpeakerLabel)) dict.Add("show-speaker-label", this.ShowSpeakerLabel);
            if(!string.IsNullOrEmpty(this.EnableChannelIdentification)) dict.Add("enable-channel-identification", this.EnableChannelIdentification);
            if(!string.IsNullOrEmpty(this.NumberOfChannels)) dict.Add("number-of-channels", this.NumberOfChannels);
            if(!string.IsNullOrEmpty(this.EnablePartialResultsStabilization)) dict.Add("enable-partial-results-stabilization", this.EnablePartialResultsStabilization);
            if(!string.IsNullOrEmpty(this.PartialResultsStability)) dict.Add("partial-results-stability", this.PartialResultsStability);
            if(!string.IsNullOrEmpty(this.ContentIdentificationType)) dict.Add("content-identification-type", this.ContentIdentificationType);
            if(!string.IsNullOrEmpty(this.ContentRedactionType)) dict.Add("content-redaction-type", this.ContentRedactionType);
            if(!string.IsNullOrEmpty(this.PiiEntityTypes)) dict.Add("pii-entity-types", this.PiiEntityTypes);
            if(!string.IsNullOrEmpty(this.LanguageModelName)) dict.Add("language-model-name", this.LanguageModelName);
            if(!string.IsNullOrEmpty(this.IdentifyLanguage)) dict.Add("identify-language", this.IdentifyLanguage);
            if (!string.IsNullOrEmpty(this.IdentifyMultipleLanguages)) dict.Add("identify-multiple-languages", this.IdentifyMultipleLanguages);
            if (!string.IsNullOrEmpty(this.LanguageOptions)) dict.Add("language-options", this.LanguageOptions);
            if(!string.IsNullOrEmpty(this.PreferredLanguage)) dict.Add("preferred-language", this.PreferredLanguage);
            if(!string.IsNullOrEmpty(this.VocabularyNames)) dict.Add("vocabulary-names", this.VocabularyNames);
            if(!string.IsNullOrEmpty(this.VocabularyFilterNames)) dict.Add("vocabulary-filter-names", this.VocabularyFilterNames);

            return dict;
        }
    }
}