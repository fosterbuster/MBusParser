// <copyright file="MbusRecord.cs" company="Poul Erik Venø Hansen">
// Copyright (c) Poul Erik Venø Hansen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using MBus.DataRecord;
using MBus.DataRecord.DataRecordHeader;

namespace MBus
{
    /// <summary>
    /// An mbus record. This is not part of the standard.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="Value"/>.</typeparam>
    public sealed class MbusRecord
    {
        /// <summary>
        /// 
        /// </summary>
        public Unit Unit { get; internal set; }

        public object Value { get; internal set; } = default;

        public int StorageNumber { get; internal set; }

        public bool IsSubUnit { get; internal set; }

        public ValueDescription ValueDescription { get; internal set; }

    }
}