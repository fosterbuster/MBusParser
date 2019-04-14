// <copyright file="ValueInformationExtensionField.cs" company="Poul Erik Venø Hansen">
// Copyright (c) Poul Erik Venø Hansen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace MBus.DataRecord.DataRecordHeader.ValueInformationBlock.Extension
{
    /// <summary>
    /// Base class for value information extension fields
    /// </summary>
    public abstract class ValueInformationExtensionField : ValueInformationField
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueInformationExtensionField"/> class.
        /// </summary>
        /// <param name="fieldByte">the fieldbyte of the VIFE</param>
        protected ValueInformationExtensionField(byte fieldByte)
            : base(fieldByte)
        {
        }
    }
}