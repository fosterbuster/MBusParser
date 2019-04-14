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
}