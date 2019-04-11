// <copyright file="KamstrupValueInformationExtensionField.cs" company="Poul Erik Venø Hansen">
// Copyright (c) Poul Erik Venø Hansen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace MBus.DataRecord.DataRecordHeader.ValueInformationBlock.Extension
{
    public sealed class KamstrupValueInformationExtensionField : ValueInformationExtensionField
    {
        public KamstrupValueInformationExtensionField(byte fieldByte)
            : base(fieldByte)
        {
        }
    }
}