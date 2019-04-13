// <copyright file="FrameType.cs" company="Poul Erik Venø Hansen">
// Copyright (c) Poul Erik Venø Hansen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using System.Collections.Generic;

namespace MBus
{
    /// <summary>
    /// The header frame type.
    /// </summary>
    public enum FrameType
    {
        /// <summary>
        /// Full frame.
        /// </summary>
        Full = 0,

        /// <summary>
        /// Format frame.
        /// </summary>
        Format = 1,

        /// <summary>
        /// Compact frame
        /// </summary>
        Compact = 2,

        /// <summary>
        /// Extended link layer.
        /// </summary>
        ExtendedLinkLayerTwoBytes = 3,

        /// <summary>
        /// Extended link layer.
        /// </summary>
        ExtendedLinkLayerEightBytes = 4,

        /// <summary>
        /// Extended link layer.
        /// </summary>
        ExtendedLinkLayerTenBytes = 5,

        /// <summary>
        /// Extended link layer.
        /// </summary>
        ExtendedLinkLayerSixteenBytes = 6,

    }

    public struct Type
    {
        public FrameType FrameType { get; set; }
        public HeaderType HeaderType { get; set; }
    }

    


    public enum newff
    {
        Full = 0x78,
        Compact = 0x79,
        Format = 0x79,
        HeaderType = HeaderType.NoData;
                    break;
                case 0x79:
                    FrameType = FrameType.Compact;
        HeaderType = HeaderType.NoData;
                    break;
                case 0x69:
                    FrameType = FrameType.Format;
        HeaderType = HeaderType.NoData;
                    break;

                // Short header
                case 0x7A:
                    FrameType = FrameType.Full;
        HeaderType = HeaderType.Short;
                    break;
                case 0x7B:
                    FrameType = FrameType.Compact;
        HeaderType = HeaderType.Short;
                    break;
                case 0x6A:
                    FrameType = FrameType.Format;
        HeaderType = HeaderType.Short;
                    break;
                // Long header
                case 0x72:
                    FrameType = FrameType.Full;
        HeaderType = HeaderType.Long;
                    break;
                case 0x73:
                    FrameType = FrameType.Compact;
        HeaderType = HeaderType.Long;
                    break;
                case 0x6B:
                    FrameType = FrameType.Format;
        HeaderType = HeaderType.Long;
                    break;
                case 0x8C:
                case 0x8D:
                case 0x8E:
                case 0x8F:
                    FrameType = FrameType.ExtendedLinkLayer;
        HeaderType = HeaderType.ExtendedLinkLayer;
        IsExtendedLinkLayer = true;
                    break;
                case 0x6C:
                case 0x6D:
                    throw new NotSupportedException(
                        $"Control information value {controlInformationField.ToHexString()} (clock sync) is not supported.");
                case 0x6E:
                case 0x6F:
                case 0x70:
                    throw new NotSupportedException(
                        $"Control information value {controlInformationField.ToHexString()} (application error from device) is not supported.");
                default:
                    throw new NotSupportedException(
                        $"Control information field value {controlInformationField.ToHexString()} is not supported.");
}
}