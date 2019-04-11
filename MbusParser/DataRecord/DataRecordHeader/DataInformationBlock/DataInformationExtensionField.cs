// <copyright file="DataInformationExtensionField.cs" company="Poul Erik Venø Hansen">
// Copyright (c) Poul Erik Venø Hansen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using System;
using MBus.Extensions;

namespace MBus.DataRecord.DataRecordHeader.DataInformationBlock
{
    /// <summary>
    /// Each DIFE (maximum ten) contains again an extension bit to show whether a further DIFE is being sent. Besides giving the next most significant bits of the storage number, DIFEs allow the transmission of information about the tariff and the subunit of the device.In this way, exactly as with the storage number, the next most significant bit or bits will be transmitted.
    /// </summary>
    public sealed class DataInformationExtensionField : InformationField
    {
        private const byte SubUnitMask = 0b0100_0000;
        private const byte TariffMask = 0b0011_0000;
        private const byte StorageNumberMask = 0b0000_1111;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataInformationExtensionField"/> class.
        /// </summary>
        /// <param name="fieldByte">the DIFE byte.</param>
        public DataInformationExtensionField(byte fieldByte)
            : base(fieldByte)
        {
        }

        /// <summary>
        /// Gets the storage number.
        /// </summary>
        public int StorageNumber => FieldByte.Mask(StorageNumberMask);

        // TODO: Find out what a sub unit is!

        /// <summary>
        /// Gets a value indicating whether the data came from a the (device) sub unit.
        /// </summary>
        public bool SubUnit => Convert.ToBoolean(FieldByte.Mask(SubUnitMask).ShiftRight(6));

        // TODO handle tariff

        /// <summary>
        /// Gets tariff.
        /// ----
        /// For each (unique) value type designation given by the following VIB at each unique time point (given 
        /// by the storage number) of each unique function(given by the function field), there might exist still
        /// various different data, measured or accumulated under different conditions.Such conditions could be
        /// time of day, various value ranges of the variable (i.e.separate storage of positive accumulated values
        /// and negative accumulated values) itself or of other signals or variables or various averaging durations.
        /// Such variables, which could not be distinguished otherwise, are made different by assigning them
        /// different values of the tariff variable in their data information block.
        /// ----
        /// NOTE This includes but is not necessarily restricted to various tariffs in a monetary sense.It is at the
        /// distinction of the manufacturer to describe for each tariff (except 0) what is different for each tariff number.Again,
        /// as with the storage numbers, all variables with the same tariff information share the same tariff associating
        /// condition.
        /// </summary>
        public Tariff Tariff => new Tariff(FieldByte.Mask(TariffMask).ShiftRight(4));
    }
}