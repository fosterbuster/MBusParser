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
