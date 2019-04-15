// <copyright file="PrimaryValueInformationField.cs" company="Poul Erik Venø Hansen">
// Copyright (c) Poul Erik Venø Hansen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using System;
using MBus.DataRecord.DataRecordHeader.DataInformationBlock;
using MBus.Extensions;

namespace MBus.DataRecord.DataRecordHeader.ValueInformationBlock
{
    /// <summary>
    /// Specifies information about the intrepretation of the data.
    /// </summary>
    public sealed class PrimaryValueInformationField : ValueInformationField
    {
        private readonly DataField _dataField;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrimaryValueInformationField"/> class.
        /// </summary>
        /// <param name="fieldByte">the VIF byte.</param>
        /// <param name="dataField">datafield used for parsing determining date correctly.</param>
        public PrimaryValueInformationField(byte fieldByte, DataField dataField)
            : base(fieldByte)
        {
            _dataField = dataField;
            Parse();
        }

        internal PrimaryValueInformation Type { get; private set; }

        private bool SetType(byte vif)
        {
            Type = (PrimaryValueInformation)vif;
            var success = Enum.IsDefined(typeof(PrimaryValueInformation), Type);
            return success;
        }

        private byte DetermineTypeAndMultiplier()
        {
            byte baseMultiplier = 0;

            if (HasExtensionBit && SetType(FieldByte))
            {
                return baseMultiplier;
            }
            else if (SetType(FieldByte.Mask(ValueInformationMask).Or(LastThreeBitsMask)))
            {
                return FieldByte.Mask(ValueInformationMask).Mask(LastThreeBitsMask);
            }
            else if (SetType(FieldByte.Mask(ValueInformationMask).Or(LastTwoBitsMask)))
            {
                return FieldByte.Mask(ValueInformationMask).Mask(LastTwoBitsMask);
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
                case PrimaryValueInformation.Date:
                    ParseDate(_dataField);
                    break;
                case PrimaryValueInformation.DateTimeGeneral:
                    ParseDate(_dataField);
                    break;
                case PrimaryValueInformation.EnergyWh:
                    Multiplier = baseMultiplier - 3;
                    Unit = Unit.Wh;
                    break;
                case PrimaryValueInformation.EnergyJoule:
                    Multiplier = baseMultiplier;
                    Unit = Unit.J;
                    break;
                case PrimaryValueInformation.Volume:
                    Multiplier = baseMultiplier - 6;
                    Unit = Unit.M3;
                    break;
                case PrimaryValueInformation.Mass:
                    Multiplier = baseMultiplier - 3;
                    Unit = Unit.Kg;
                    break;
                case PrimaryValueInformation.PowerW:
                    Multiplier = baseMultiplier - 3;
                    Unit = Unit.W;
                    break;
                case PrimaryValueInformation.PowerJh:
                    Multiplier = baseMultiplier;
                    Unit = Unit.Jh;
                    break;
                case PrimaryValueInformation.VolumeFlow:
                    Multiplier = baseMultiplier - 6;
                    Unit = Unit.M3h;
                    break;
                case PrimaryValueInformation.VolumeFlowExt:
                    Multiplier = baseMultiplier - 7;
                    Unit = Unit.M3min;
                    break;
                case PrimaryValueInformation.VolumeFlowExtS:
                    Multiplier = baseMultiplier - 9;
                    Unit = Unit.M3s;
                    break;
                case PrimaryValueInformation.MassFlow:
                    Multiplier = baseMultiplier - 3;
                    Unit = Unit.Kgh;
                    break;
                case PrimaryValueInformation.OnTime:
                    Type = PrimaryValueInformation.OnTime;
                    ParseTime(baseMultiplier);
                    break;
                case PrimaryValueInformation.InletFlowTemperature:
                case PrimaryValueInformation.ReturnFlowTemperature:
                case PrimaryValueInformation.ExternalTemperature:
                    Multiplier = baseMultiplier - 3;
                    Unit = Unit.C;
                    break;
                case PrimaryValueInformation.TemperatureDifference:
                    Multiplier = baseMultiplier - 3;
                    Unit = Unit.K;
                    break;
                case PrimaryValueInformation.Pressure:
                    Type = PrimaryValueInformation.Pressure;
                    Multiplier = baseMultiplier - 3;
                    Unit = Unit.Bar;
                    break;
            }
        }

        private void ParseDate(in DataField datafield)
        {
            var mapped = (TimeMask)datafield;
            switch (mapped)
            {
                case TimeMask.Date:
                    Unit = Unit.Date;
                    break;
                case TimeMask.DateTime:
                    Type = PrimaryValueInformation.DateTime;
                    Unit = Unit.DateTime;
                    break;
                case TimeMask.TimeExtended:
                    Type = PrimaryValueInformation.ExtendedTime;
                    Unit = Unit.DateTime;
                    break;
                case TimeMask.DateTimeExtended:
                    Type = PrimaryValueInformation.ExtendedDateTime;
                    Unit = Unit.DateTimeSecondary;
                    break;
            }
        }

        private void ParseTime(in byte value)
        {
            switch (value)
            {
                case 0:
                    Unit = Unit.Seconds;
                    break;
                case 1:
                    Unit = Unit.Minutes;
                    break;
                case 2:
                    Unit = Unit.Hours;
                    break;
                case 3:
                    Unit = Unit.Days;
                    break;
            }

        }
    }
}