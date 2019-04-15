// <copyright file="OrthogonalValueInformationExtensionField.cs" company="Poul Erik Venø Hansen">
// Copyright (c) Poul Erik Venø Hansen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using System;
using MBus.Extensions;

namespace MBus.DataRecord.DataRecordHeader.ValueInformationBlock.Extension
{
    /// <summary>
    /// This code follows immediately the VIF or the VIFE (in case of code extension) and modifies its meaning.
    /// </summary>
    public sealed class OrthogonalValueInformationExtensionField : ValueInformationExtensionField
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrthogonalValueInformationExtensionField"/> class.
        /// </summary>
        /// <param name="fieldByte">the byte of the orthogonal vife.</param>
        public OrthogonalValueInformationExtensionField(byte fieldByte)
            : base(fieldByte)
        {
            Parse();
        }

        internal OrthogonalValueInformationExtension Type { get; private set; }

        private bool SetType(byte vif)
        {
            Type = (OrthogonalValueInformationExtension)vif;
            var success = Enum.IsDefined(typeof(OrthogonalValueInformationExtension), Type);
            return success;
        }

        private byte DetermineTypeAndMultiplier()
        {
            byte baseMultiplier = 0;
            if (SetType(FieldByte.Mask(ValueInformationMask).Or(LastThreeBitsMask)))
            {
                return FieldByte.Mask(ValueInformationMask).Mask(LastThreeBitsMask);
            }
            else if (SetType(FieldByte.Mask(ValueInformationMask).Or(LastTwoBitsMask)))
            {
                return FieldByte.Mask(ValueInformationMask).Mask(LastTwoBitsMask);
            }
            else if (SetType(FieldByte.Mask(ValueInformationMask).Or(LastBitMask)))
            {
                return FieldByte.Mask(ValueInformationMask).Mask(LastBitMask);
            }
            else if (SetType(FieldByte.Mask(ValueInformationMask)))
            {
                return baseMultiplier;
            }
            else
            {
                // TODO HANDLE ERROR
                return baseMultiplier;
            }
        }

        private void Parse()
        {
            var baseMultiplier = DetermineTypeAndMultiplier();

            switch (Type)
            {
                case OrthogonalValueInformationExtension.MultiplicativeCorrectionFactorMinusSix:
                    Multiplier = baseMultiplier - 6;
                    break;
            }
        }
    }
}