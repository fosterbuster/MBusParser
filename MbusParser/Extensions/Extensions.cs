using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MBus.DataRecord;

namespace MBus.Extensions
{
    /// <summary>
    /// General extensions
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Aka easymode.
        /// </summary>
        /// <param name="dataRecord">the datarecord to make easy.</param>
        /// <returns></returns>
        public static MbusRecord ToMBusRecord(this VariableDataRecord dataRecord)
        {
            return new MbusRecord
            {
                IsSubUnit = dataRecord.DataInformationFieldExtensions.Last().SubUnit,
                StorageNumber = dataRecord.DataInformationFieldExtensions.Last().StorageNumber,
                Unit = dataRecord.DataBlock.Unit,
                Value = dataRecord.DataBlock.Value,
            };
        }
    }
}