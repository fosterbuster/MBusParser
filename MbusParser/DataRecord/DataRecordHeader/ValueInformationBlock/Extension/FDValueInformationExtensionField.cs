// <copyright file="FDValueInformationExtensionField.cs" company="Poul Erik Venø Hansen">
// Copyright (c) Poul Erik Venø Hansen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using MBus.Extensions;
using System;

namespace MBus.DataRecord.DataRecordHeader.ValueInformationBlock.Extension
{
    /// <summary>
    /// Main VIFE-code extension.
    /// </summary>
    public sealed class FDValueInformationExtensionField : ValueInformationExtensionField
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FDValueInformationExtensionField"/> class.
        /// </summary>
        /// <param name="fieldByte">the field byte of the FD-VIFE.</param>
        public FDValueInformationExtensionField(byte fieldByte)
            : base(fieldByte)
        {
            Parse();
        }

        internal FDValueInformationExtension Type { get; private set; }

        private bool SetType(byte vif)
        {
            Type = (FDValueInformationExtension)vif;
            var success = Enum.IsDefined(typeof(FDValueInformationExtension), Type);
            return success;
        }

        private byte DetermineTypeAndMultiplier()
        {
            byte baseMultiplier = 0;

            if (SetType(FieldByte.Mask(ValueInformationMask).Or(LastFourBitsMask)))
            {
                return FieldByte.Mask(ValueInformationMask).Mask(LastFourBitsMask);
            }
            else if (SetType(FieldByte.Mask(ValueInformationMask).Or(LastThreeBitsMask)))
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
                case FDValueInformationExtension.Volts:
                    Multiplier = baseMultiplier - 9;
                    Unit = Unit.V;
                    break;
                case FDValueInformationExtension.Ampere:
                    Multiplier = baseMultiplier - 12;
                    Unit = Unit.A;
                    break;
                case FDValueInformationExtension.CurrencyCredit:
                case FDValueInformationExtension.CurrencyDebit:
                    Multiplier = baseMultiplier - 3;
                    break;
            }
        }
    }
}