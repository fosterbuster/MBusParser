// <copyright file="ControlInformation.cs" company="Poul Erik Venø Hansen">
// Copyright (c) Poul Erik Venø Hansen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using System;
using MBus.Extensions;

namespace MBus.Header
{
    /// <summary>
    /// A 1-byte information field used to declare the structure of the following block.
    /// </summary>
    public sealed class ControlInformation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ControlInformation"/> class.
        /// </summary>
        /// <param name="controlInformationField">ci field.</param>
        public ControlInformation(byte controlInformationField)
        {
            switch (controlInformationField)
            {
                case 0x78:
                    FrameType = FrameType.Full;
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

        /// <summary>
        /// Gets the frame type.
        /// </summary>
        public FrameType FrameType { get; }

        /// <summary>
        /// Gets the header type.
        /// </summary>
        public HeaderType HeaderType { get; }

        /// <summary>
        /// A value indicating whether an extended link layer follows the header.
        /// </summary>
        public bool IsExtendedLinkLayer { get; }

    }
}
