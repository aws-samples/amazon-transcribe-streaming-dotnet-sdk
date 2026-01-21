// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
// SPDX-License-Identifier: Apache-2.0

namespace Amazon.TranscribeStreamingService.Models {
    
    public class ChannelDefinition {
        public int ChannelId { get; set; }
        public string? ParticipantRole { get; set; }
    }

    public class PostCallAnalyticsSettings {
        public string? ContentRedactionOutput { get; set; }
        public string? DataAccessRoleArn { get; set; }
        public string? OutputEncryptionKMSKeyId { get; set; }
        public string? OutputLocation { get; set; }
    }

    public class ConfigurationEvent {
        public List<ChannelDefinition>? ChannelDefinitions { get; set; }
        public PostCallAnalyticsSettings? PostCallAnalyticsSettings { get; set; }
    }
}
