// <copyright file="FBValueInformationExtensionField.cs" company="Poul Erik Venø Hansen">
// Copyright (c) Poul Erik Venø Hansen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using MBus.Extensions;
using System;

namespace MBus.DataRecord.DataRecordHeader.ValueInformationBlock.Extension
{
    /// <summary>
    /// Alternate VIFE-code extension.
    /// </summary>
    public sealed class FBValueInformationExtensionField : ValueInformationExtensionField
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FBValueInformationExtensionField"/> class.
        /// </summary>
        /// <param name="fieldByte">the field byte of the FB-VIFE.</param>
        public FBValueInformationExtensionField(byte fieldByte)
            : base(fieldByte)
        {
            Parse();
        }

        internal FBValueInformationExtension Type { get; private set; }

        private bool SetType(byte vif)
        {
            Type = (FBValueInformationExtension)vif;
            var success = Enum.IsDefined(typeof(FBValueInformationExtension), Type);
            return success;
        }

        private byte DetermineTypeAndMultiplier()
        {
            byte baseMultiplier = 0;

            if (SetType(FieldByte.Mask(ValueInformationMask)))
            {
                return baseMultiplier;
            }
            else if (SetType(FieldByte.Mask(ValueInformationMask).Or(LastBitMask)))
            {
                return FieldByte.Mask(ValueInformationMask).Mask(LastBitMask);
            }
            else if (SetType(FieldByte.Mask(ValueInformationMask).Or(LastTwoBitsMask)))
            {
                return FieldByte.Mask(ValueInformationMask).Mask(LastTwoBitsMask);
            }
            else if (SetType(FieldByte.Mask(ValueInformationMask).Or(LastThreeBitsMask)))
            {
                return FieldByte.Mask(ValueInformationMask).Mask(LastThreeBitsMask);
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
                case FBValueInformationExtension.EnergyMWh:
                    Multiplier = ((baseMultiplier - 1) + 6);
                    Unit = Unit.Wh;
                    break;
                case FBValueInformationExtension.ReactiveEnergy:
                    Unit = Unit.VARh;
                    Multiplier = baseMultiplier + 3;
                    break;
                case FBValueInformationExtension.ApparentEnergy:
                    Unit = Unit.VAh;
                    Multiplier = baseMultiplier + 3;
                    break;
                case FBValueInformationExtension.EnergyGj:
                    Unit = Unit.J;
                    Multiplier = (baseMultiplier - 1) + 9;
                    break;
                case FBValueInformationExtension.EnergyMcal:
                    Unit = Unit.Cal;
                    Multiplier = (baseMultiplier - 1) + 6;
                    break;
                case FBValueInformationExtension.VolumeCubicMeter:
                    Unit = Unit.M3;
                    Multiplier = (baseMultiplier + 2);
                    break;
                case FBValueInformationExtension.Mass:
                    Unit = Unit.Kg;
                    //A tonne is a kilo kilograms
                    Multiplier = (baseMultiplier + 2) + 3;
                    break;
                case FBValueInformationExtension.RelativeHumidity:
                    Unit = Unit.Percentage;
                    Multiplier = baseMultiplier - 1;
                    break;
                case FBValueInformationExtension.VolumeCubicFeet:
                    throw new NotSupportedException("Imperial Measurements are not supported.");
                case FBValueInformationExtension.VolumeCubicFeet1:
                    throw new NotSupportedException("Imperial Measurements are not supported.");
                case FBValueInformationExtension.PowerMw:
                    Unit = Unit.W;
                    Multiplier = (baseMultiplier - 1) + 6;
                    break;
                case FBValueInformationExtension.PowerPhaseUU:
                case FBValueInformationExtension.PowerPhaseUI:
                    Unit = Unit.Rad;
                    break;
                case FBValueInformationExtension.PowerGjh:
                    Unit = Unit.Jh;
                    Multiplier = (baseMultiplier - 1) + 9;
                    break;
                case FBValueInformationExtension.CumulativeMaxActivePower:
                    Unit = Unit.W;
                    Multiplier = baseMultiplier - 3;
                    break;
                case FBValueInformationExtension.ReactivePower:
                    Unit = Unit.VAR;
                    Multiplier = (baseMultiplier - 3) + 3;
                    break;
                case FBValueInformationExtension.Frequency:
                    Unit = Unit.Hz;
                    Multiplier = baseMultiplier - 3;
                    break;
                case FBValueInformationExtension.ApparentPower:
                    Unit = Unit.VA;
                    Multiplier = baseMultiplier - 3 + 3;
                    break;
                case FBValueInformationExtension.ColdWarmTemperatureLimit:
                    Unit = Unit.C;
                    Multiplier = baseMultiplier - 3;
                    break;
            }
        }
    }
}