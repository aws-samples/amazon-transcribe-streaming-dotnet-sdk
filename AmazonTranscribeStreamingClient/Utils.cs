// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
// SPDX-License-Identifier: Apache-2.0

namespace Amazon.TranscribeStreamingService {

    public class Utils {

        public static ushort SwapBytes(ushort x)
        {
            return (ushort)((ushort)((x & 0xff) << 8) | ((x >> 8) & 0xff));
        }

        public static uint SwapBytes(uint x)
        {
            return ((x & 0x000000ff) << 24) +
                ((x & 0x0000ff00) << 8) +
                ((x & 0x00ff0000) >> 8) +
                ((x & 0xff000000) >> 24);
        }

        public static ulong SwapBytes(ulong value)
        {
            ulong uvalue = value;
            ulong swapped =
                ((0x00000000000000FF) & (uvalue >> 56)
                | (0x000000000000FF00) & (uvalue >> 40)
                | (0x0000000000FF0000) & (uvalue >> 24)
                | (0x00000000FF000000) & (uvalue >> 8)
                | (0x000000FF00000000) & (uvalue << 8)
                | (0x0000FF0000000000) & (uvalue << 24)
                | (0x00FF000000000000) & (uvalue << 40)
                | (0xFF00000000000000) & (uvalue << 56));
            return swapped;
        }
    }
}
