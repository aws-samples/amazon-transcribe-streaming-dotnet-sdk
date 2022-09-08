// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
// SPDX-License-Identifier: Apache-2.0

namespace Amazon.TranscribeStreamingService.Extensions {
    public static class DictionaryExtensions
    {
        public static void Update<K,V>(this IDictionary<K, V> me, IDictionary<K, V> other)
        {
            foreach (var x in other)
            {
                me[x.Key] = x.Value;
            }
        }
    }
}