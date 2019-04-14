// <copyright file="DataBlock.cs" company="Poul Erik Venø Hansen">
// Copyright (c) Poul Erik Venø Hansen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MBus.DataRecord.DataRecordHeader;
using MBus.DataRecord.DataRecordHeader.DataInformationBlock;
using MBus.DataRecord.DataRecordHeader.ValueInformationBlock;
using MBus.DataRecord.DataRecordHeader.ValueInformationBlock.Extension;
using MBus.Extensions;

namespace MBus.DataRecord
{
    /// <summary>
    /// An mbus datablock.
    /// </summary>
    public sealed class DataBlock
    {
        public Unit Unit { get; private set; }

        public object Value { get; private set; }

        public ValueDescription ValueDescription { get; private set; }

        private readonly byte[] _data;

        internal DataInformationField DataInformationField { get; }

        /// <summary>
        /// Gets DIFEs.
        /// </summary>
        internal List<DataInformationExtensionField> DataInformationFieldExtensions { get; }

        /// <summary>
        /// Gets VIF.
        /// </summary>
        internal PrimaryValueInformationField ValueInformationField { get; }

        /// <summary>
        /// Gets VIFEs.
        /// </summary>
        internal List<ValueInformationExtensionField> ValueInformationFieldExtensions { get; }

        public DataBlock(byte[] data, DataInformationField dataInformationField, List<DataInformationExtensionField> dataInformationFieldExtensions, PrimaryValueInformationField valueInformationField, List<ValueInformationExtensionField> valueInformationFieldExtensions)
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
            DataInformationField = dataInformationField ?? throw new ArgumentNullException(nameof(dataInformationField));
            DataInformationFieldExtensions = dataInformationFieldExtensions ?? throw new ArgumentNullException(nameof(dataInformationFieldExtensions));
            ValueInformationField = valueInformationField ?? throw new ArgumentNullException(nameof(valueInformationField));
            ValueInformationFieldExtensions = valueInformationFieldExtensions ?? throw new ArgumentNullException(nameof(valueInformationFieldExtensions));

            Parse();
        }

        private void Parse()
        {
            ValueDescription = FindDecription();
            Unit = FindUnit();
            var multiplier = FindMultiplier();
            Value = ParseValue(multiplier);
        }

        private int FindMultiplier()
        {
            return ValueInformationField.Multiplier + ValueInformationFieldExtensions.Sum(x => x.Multiplier);
        }

        private Unit FindUnit()
        {
            var lastVife = ValueInformationFieldExtensions.LastOrDefault(x => x.Unit != Unit.None);

            if (lastVife != null && lastVife.Unit != Unit.None)
            {
                return lastVife.Unit;
            }

            return ValueInformationField.Unit;
        }

        private object? ParseValue(int multiplier)
        {
            var valueType = DataInformationField.DataField;

            if (Unit == Unit.Date || Unit == Unit.DateTime || Unit == Unit.DateTimeSecondary || Unit == Unit.Time)
            {
                return ParseDateTime();
            }

            switch (valueType)
            {
                case DataField.NoData:
                    return null;
                case DataField.EightBitInteger:
                case DataField.SixteenBitInteger:
                case DataField.TwentyFourBitInteger:
                case DataField.ThirtyTwoBitInteger:
                case DataField.FourtyEightBitInteger:
                case DataField.SixtyFourBitInteger:
                    return ParseInteger(multiplier);
                case DataField.ThirtyTwoBitReal:
                    return ParseReal(multiplier);
                case DataField.TwoDigitBinaryCodedDecimal:
                case DataField.FourDigitBinaryCodedDecimal:
                case DataField.SixDigitBinaryCodedDecimal:
                case DataField.EightDigitBinaryCodedDecimal:
                case DataField.TwelveDigitBinaryCodedDecimal:
                    return ParseBCD(multiplier);
                case DataField.VariableLength:
                    return ParseString(multiplier);
                default:
                    break;
            }

            return null;
        }

        private DateTime ParseDateTime()
        {
            return DateTime.MinValue;
        }

        private string ParseString(int multiplier)
        {
            throw new NotImplementedException();
        }

        private double ParseBCD(int multiplier)
        {
            return long.Parse(_data.Reverse().ToArray().ToHexString()) * Math.Pow(10, multiplier);
        }

        private float ParseReal(int multiplier)
        {
            return Convert.ToSingle(ValueAsLong() * Math.Pow(10, multiplier));
        }

        private double ParseInteger(int multiplier)
        {
            return ValueAsLong() * Math.Pow(10, multiplier);
        }

        private long ValueAsLong()
        {
            var length = _data.Length;

            switch (length)
            {
                case 1: return _data[0];
                case 2: return BitConverter.ToInt16(_data.Reverse().ToArray(), 0);
                case 3: return BitConverter.ToInt32(new byte[1].Concat(_data).Reverse().ToArray(), 0);
                case 4: return BitConverter.ToInt32(_data.Reverse().ToArray(), 0);
                case 6: return BitConverter.ToInt64(new byte[2].Concat(_data).Reverse().ToArray(), 0);
                case 8: return BitConverter.ToInt64(_data.Reverse().ToArray(), 0);
                default:
                    throw new InvalidOperationException(":(");
            }
        }

        private ValueDescription FindDecription()
        {
            var vife = ValueInformationFieldExtensions.LastOrDefault(x => x.Unit != Unit.None);


            if (vife != null && vife is FBValueInformationExtensionField fbVife)
            {
                return GetFbDescription(fbVife.Type);
            }

            if (vife != null && vife is FDValueInformationExtensionField fdVife)
            {
                return GetFdDescription(fdVife.Type);
            }

            if (vife != null && vife is KamstrupValueInformationExtensionField kamstrupVife)
            {
                //return GetKamstrupDescription(kamstrupVife.Type);
            }

            var vifType = ValueInformationField.Type;

            switch (vifType)
            {
                case PrimaryValueInformation.EnergyWh:
                case PrimaryValueInformation.EnergyJoule:
                    return ValueDescription.Energy;
                case PrimaryValueInformation.Volume:
                    return ValueDescription.Volume;
                case PrimaryValueInformation.Mass:
                    return ValueDescription.Mass;
                case PrimaryValueInformation.OnTime:
                case PrimaryValueInformation.OperatingTime:
                case PrimaryValueInformation.Date:
                case PrimaryValueInformation.DateTimeGeneral:
                    return ValueDescription.Time;
                case PrimaryValueInformation.PowerW:
                case PrimaryValueInformation.PowerJh:
                    return ValueDescription.Power;
                case PrimaryValueInformation.VolumeFlow:
                case PrimaryValueInformation.VolumeFlowExt:
                case PrimaryValueInformation.VolumeFlowExtS:
                    return ValueDescription.VolumeFlow;
                case PrimaryValueInformation.MassFlow:
                    return ValueDescription.MassFlow;
                case PrimaryValueInformation.InletFlowTemperature:
                    return ValueDescription.InletFlowTemperature;
                case PrimaryValueInformation.ReturnFlowTemperature:
                    return ValueDescription.ReturnFlowTemperature;
                case PrimaryValueInformation.TemperatureDifference:
                    return ValueDescription.TemperatureDifference;
                case PrimaryValueInformation.ExternalTemperature:
                    return ValueDescription.ExternalTemperature;
                case PrimaryValueInformation.Pressure:
                    return ValueDescription.Pressure;
                case PrimaryValueInformation.AveragingDuration:
                    return ValueDescription.AveragingDuration;
                case PrimaryValueInformation.ActualityDuration:
                    return ValueDescription.ActualityDuration;

                case PrimaryValueInformation.UnitsForHCA:
                case PrimaryValueInformation.ReservedForFutureThirdTableOfValueInformationExtensions:
                case PrimaryValueInformation.FabricationNumber:
                case PrimaryValueInformation.Identification:
                case PrimaryValueInformation.Address:
                case PrimaryValueInformation.FBValueInformationExtension:
                case PrimaryValueInformation.ValueInformationInFollowingString:
                case PrimaryValueInformation.FDValueInformationExtension:
                case PrimaryValueInformation.ReservedForThirdExtensionOfValueInformationCodes:
                case PrimaryValueInformation.AnyValueInformation:
                case PrimaryValueInformation.ManufacturerSpecific:
                default:
                    break;
            }

            return ValueDescription.None;
        }

        private ValueDescription GetFbDescription(FBValueInformationExtension vife)
        {
            switch (vife)
            {
                case FBValueInformationExtension.EnergyMWh:
                case FBValueInformationExtension.EnergyGj:
                case FBValueInformationExtension.EnergyMcal:
                    return ValueDescription.Energy;
                case FBValueInformationExtension.ReactiveEnergy:
                    return ValueDescription.ReactiveEnergy;
                case FBValueInformationExtension.ApparentEnergy:
                    return ValueDescription.ApperentEnergy;
                case FBValueInformationExtension.VolumeCubicMeter:
                    return ValueDescription.Volume;
                case FBValueInformationExtension.ReactivePower:
                    return ValueDescription.ReactivePower;
                case FBValueInformationExtension.Mass:
                    return ValueDescription.Mass;
                case FBValueInformationExtension.RelativeHumidity:
                    return ValueDescription.RelativeHumidity;
                //case FBValueInformationExtension.VolumeCubicFeet:
                //   break;
                //case FBValueInformationExtension.VolumeCubicFeet1:
                //break;
                case FBValueInformationExtension.PowerMw:
                case FBValueInformationExtension.PowerGjh:
                    return ValueDescription.Power;
                case FBValueInformationExtension.PowerPhaseUU:
                    return ValueDescription.PhasePotentialToPotential;
                case FBValueInformationExtension.PowerPhaseUI:
                    return ValueDescription.PhasePotentialToCurrent;
                case FBValueInformationExtension.Frequency:
                    return ValueDescription.Frequency;
                case FBValueInformationExtension.ApparentPower:
                    return ValueDescription.ApperentPower;

                    //case FBValueInformationExtension.ColdWarmTemperatureLimit:
                    //   break;
                    //case FBValueInformationExtension.CumulativeMaxActivePower:
                    //   break;
            }

            return ValueDescription.None;
        }

        private ValueDescription GetFdDescription(FDValueInformationExtension vife)
        {
            switch (vife)
            {
                case FDValueInformationExtension.Volts:

                    return ValueDescription.ElectricalPotential;
                case FDValueInformationExtension.Ampere:
                    return ValueDescription.ElectricalCurrent;
            }

            return ValueDescription.None;
        }
    }
}