// <copyright file="HeaderType.cs" company="Poul Erik Venø Hansen">
// Copyright (c) Poul Erik Venø Hansen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace MBus
{
    /// <summary>
    /// mbus header type.
    /// </summary>
    public enum HeaderType
    {
        /// <summary>
        /// No data in header.
        /// </summary>
        NoData = 0,

        /// <summary>
        /// Short header.
        /// </summary>
        Short = 1,

        /// <summary>
        /// Long header.
        /// </summary>
        Long = 2,

        /// <summary>
        /// Header is extended link layer.
        /// </summary>
        ExtendedLinkLayer = 3,
    }
}