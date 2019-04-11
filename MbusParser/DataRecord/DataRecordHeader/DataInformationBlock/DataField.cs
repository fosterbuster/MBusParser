// <copyright file="DataField.cs" company="Poul Erik Venø Hansen">
// Copyright (c) Poul Erik Venø Hansen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace MBus.DataRecord.DataRecordHeader.DataInformationBlock
{
    /// <summary>
    /// The data field shows how the data from the master shall be interpreted in respect of length and coding.
    /// </summary>
    public enum DataField
    {
        /// <summary>
        /// No data.
        /// </summary>
        NoData = 0b0000,

        /// <summary>
        /// 8 bit integer/binary.
        /// </summary>
        EightBitInteger = 0b0001,

        /// <summary>
        /// 16 bit integer/binary.
        /// </summary>
        SixteenBitInteger = 0b0010,

        /// <summary>
        /// 24 bit integer/binary.
        /// </summary>
        TwentyFourBitInteger = 0b0011,

        /// <summary>
        /// 32 bit integer/binary.
        /// </summary>
        ThirtyTwoBitInteger = 0b1000,

        /// <summary>
        /// 32 bit real.
        /// </summary>
        ThirtyTwoBitReal = 0b0101,

        /// <summary>
        /// 48 bit integer/binary.
        /// </summary>
        FourtyEightBitInteger = 0b0110,

        /// <summary>
        /// 64 bit integer/binary.
        /// </summary>
        SixtyFourBitInteger = 0b0111,

        /// <summary>
        /// Selection for readout.
        /// </summary>
        SelectionForReadout = 0b1000,

        /// <summary>
        /// 2 digit BCD.
        /// </summary>
        TwoDigitBinaryCodedDecimal = 0b1001,

        /// <summary>
        /// 4 digit BCD.
        /// </summary>
        FourDigitBinaryCodedDecimal = 0b1010,

        /// <summary>
        /// 6 digit BCD.
        /// </summary>
        SixDigitBinaryCodedDecimal = 0b1011,

        /// <summary>
        /// 8 digit BCD.
        /// </summary>
        EightDigitBinaryCodedDecimal = 0b1100,

        // Yeah, for some reason 10 digit BCD is not in the standard :O

        /// <summary>
        /// 12 digit BCD.
        /// </summary>
        TwelveDigitBinaryCodedDecimal = 0b1110,

        /// <summary>
        /// With data field = `0b1101´ several data types with variable length can be used. The length of the data is given after the DRH with the first byte of real data.
        /// </summary>
        VariableLength = 0b1101,

        /// <summary>
        /// Special functions.
        /// </summary>
        SpecialFunctions = 0b1111,
    }
}