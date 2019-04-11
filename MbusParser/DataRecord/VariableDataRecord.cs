using System;
using System.Collections.Generic;
using MBus.DataRecord.DataRecordHeader.DataInformationBlock;
using MBus.DataRecord.DataRecordHeader.ValueInformationBlock;
using MBus.DataRecord.DataRecordHeader.ValueInformationBlock.Extension;

namespace MBus.DataRecord
{
    /// <summary>
    /// An MBus DataRecord.
    /// </summary>
    public class VariableDataRecord
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VariableDataRecord"/> class.
        /// </summary>
        /// <param name="dataInformationField"></param>
        /// <param name="dataInformationFieldExtensions"></param>
        /// <param name="valueInformationField"></param>
        /// <param name="valueInformationFieldExtensions"></param>
        /// <param name="dataBlock"></param>
        /// <param name="rssi"></param>
        public VariableDataRecord(DataBlock dataBlock, double? rssi = null)
        {
            DataBlock = dataBlock ?? throw new ArgumentNullException(nameof(dataBlock));
            Rssi = rssi;
        }

        /// <summary>
        /// Gets DIF.
        /// </summary>
        public DataInformationField DataInformationField => DataBlock.DataInformationField;

        /// <summary>
        /// Gets DIFEs.
        /// </summary>
        public List<DataInformationExtensionField> DataInformationFieldExtensions => DataBlock.DataInformationFieldExtensions;

        /// <summary>
        /// Gets VIF.
        /// </summary>
        public PrimaryValueInformationField ValueInformationField => DataBlock.ValueInformationField;

        /// <summary>
        /// Gets VIFEs.
        /// </summary>
        public List<ValueInformationExtensionField> ValueInformationFieldExtensions => DataBlock.ValueInformationFieldExtensions;

        /// <summary>
        /// Gets the data block.
        /// </summary>
        public DataBlock DataBlock { get; }

        /// <summary>
        /// Gets a Relative Signal Strength Indicator. Present if the parsed packet was from a wireless mbus.
        /// </summary>
        public double? Rssi { get; internal set; }
    }
}
