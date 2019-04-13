// <copyright file="DataInformationField.cs" company="Poul Erik Venø Hansen">
// Copyright (c) Poul Erik Venø Hansen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using MBus.Extensions;

namespace MBus.DataRecord.DataRecordHeader.DataInformationBlock
{
    /// <summary>
    /// Specifies information about the data it self.
    /// </summary>
    public sealed class DataInformationField : InformationField
    {
        private const byte DataFieldMask = 0b0000_1111;
        private const byte FunctionFieldMask = 0b0011_0000;
        private const byte StorageNumberMask = 0b0100_0000;

        private int? _dataLength;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataInformationField"/> class.
        /// </summary>
        /// <param name="fieldByte">the byte of the DIF</param>
        public DataInformationField(byte fieldByte)
            : base(fieldByte)
        {
        }

        /// <summary>
        /// Gets a value indicating the encoding of the data.
        /// </summary>
        public DataField DataField => (DataField)FieldByte.Mask(DataFieldMask);

        /// <summary>
        /// Gets a value indicating the function of the data.
        /// </summary>
        public FunctionField FunctionField => (FunctionField)FieldByte.Mask(FunctionFieldMask).ShiftRight(4);

        /// <summary>
        /// Gets a value indicating whether this bit is the least significant bit of the storage number and allows therefore the storage numbers 0 and 1 to be coded.If storage numbers higher than “1” are needed, following (optional) DIFE´s contain the higher bits.The storage number 0 signals a current value.
        /// </summary>
        public int StorageNumber => FieldByte.Mask(StorageNumberMask).ShiftRight(6);

        /// <summary>
        /// Gets or sets the length of the data block.
        /// </summary>
        /// <returns>the length of the data, if known.</returns>
        public int? DataLength
        {
            get => _dataLength ?? GetDataLength();
            set => _dataLength = value;
        }

        private int? GetDataLength()
        {
            switch (DataField)
            {
                case DataField.NoData:
                    return 0;
                case DataField.EightBitInteger:
                    return 8 / 8;
                case DataField.SixteenBitInteger:
                    return 16 / 8;
                case DataField.TwentyFourBitInteger:
                    return 24 / 8;
                case DataField.ThirtyTwoBitInteger:
                    return 32 / 8;
                case DataField.ThirtyTwoBitReal:
                    return 32 / 8;
                case DataField.FourtyEightBitInteger:
                    return 48 / 8;
                case DataField.SixtyFourBitInteger:
                    return 64 / 8;
                case DataField.TwoDigitBinaryCodedDecimal:
                    return 1;
                case DataField.FourDigitBinaryCodedDecimal:
                    return 2;
                case DataField.SixDigitBinaryCodedDecimal:
                    return 3;
                case DataField.EightDigitBinaryCodedDecimal:
                    return 4;
                case DataField.TwelveDigitBinaryCodedDecimal:
                    return 6;
                case DataField.VariableLength:
                    return -1;
                case DataField.SpecialFunctions:
                    return -1;
                default:
                    return null;
            }
        }
    }
}