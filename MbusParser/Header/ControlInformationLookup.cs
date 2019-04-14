using System;
using System.Collections.Generic;
using System.Text;

namespace MBus.Header
{
    /// <summary>
    /// Helper class for looking up <see cref="HeaderType"/> and <see cref="FrameType"/>
    /// </summary>
    public static class ControlInformationLookup
    {
        private static readonly IReadOnlyDictionary<byte, (FrameType frameType, HeaderType headerType)> _lookup = new Dictionary<byte, (FrameType frameType, HeaderType headerType)>
        {
            // No data
            { 0x78, (FrameType.Full, HeaderType.NoData) },
            { 0x79, (FrameType.Compact, HeaderType.NoData) },
            { 0x69, (FrameType.Format, HeaderType.NoData) },

            // Short header
            { 0x7A, (FrameType.Full, HeaderType.Short) },
            { 0x7B, (FrameType.Compact, HeaderType.Short) },
            { 0x6A, (FrameType.Format, HeaderType.Short) },

            // Long header
            { 0x72, (FrameType.Full, HeaderType.Long) },
            { 0x73, (FrameType.Compact, HeaderType.Long) },
            { 0x6B, (FrameType.Format, HeaderType.Long) },

            // Extended Link Layer
            { 0x8C, (FrameType.ExtendedLinkLayerTwoBytes, HeaderType.ExtendedLinkLayer) },
            { 0x8D, (FrameType.ExtendedLinkLayerEightBytes, HeaderType.ExtendedLinkLayer) },
            { 0x8E, (FrameType.ExtendedLinkLayerTenBytes, HeaderType.ExtendedLinkLayer) },
            { 0x8F, (FrameType.ExtendedLinkLayerSixteenBytes, HeaderType.ExtendedLinkLayer) },
        };

        /// <summary>
        /// Find <see cref="FrameType"/> and <see cref="HeaderType"/> by <paramref name="field"/>
        /// </summary>
        /// <param name="field">The CI-field byte.</param>
        /// <returns>A tuple of <see cref="FrameType"/> and <see cref="HeaderType"/></returns>
        public static (FrameType frameType, HeaderType headerType) Find(byte field)
        {
            return _lookup.GetValueOrDefault(field);
        }

        public static int HeaderLength(byte field)
        {
            var (frame, header) = Find(field);

            switch (header)
            {
                case HeaderType.NoData:
                    return 0;
                case HeaderType.Short:
                    return 4;
                case HeaderType.Long:
                    return 12;
                case HeaderType.ExtendedLinkLayer:
                    switch (frame)
                    {
                        case FrameType.ExtendedLinkLayerTwoBytes:
                            return 2;
                        case FrameType.ExtendedLinkLayerEightBytes:
                            return 8;
                        case FrameType.ExtendedLinkLayerTenBytes:
                            return 10;
                        case FrameType.ExtendedLinkLayerSixteenBytes:
                            return 16;
                    }

                    break;
            }

            // Should never go here!
            return -1;
        }
    }
}