// <copyright file="FunctionField.cs" company="Poul Erik Venø Hansen">
// Copyright (c) Poul Erik Venø Hansen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace MBus.DataRecord.DataRecordHeader.DataInformationBlock
{
    /// <summary>
    /// The function field gives the type of data.
    /// </summary>
    public enum FunctionField
    {
        /// <summary>
        /// Instantaneous value.
        /// </summary>
        Instantaneous = 0b00,

        /// <summary>
        /// Minimum value.
        /// </summary>
        Minimum = 0b10,

        /// <summary>
        /// Maximum value.
        /// </summary>
        Maximum = 0b01,

        /// <summary>
        /// Value during error state.
        /// </summary>
        Error = 0b11,
    }
}