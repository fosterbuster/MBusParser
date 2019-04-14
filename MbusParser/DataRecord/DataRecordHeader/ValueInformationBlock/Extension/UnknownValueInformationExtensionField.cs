using System;
using System.Collections.Generic;
using System.Text;

namespace MBus.DataRecord.DataRecordHeader.ValueInformationBlock.Extension
{
    public class UnknownValueInformationExtensionField : ValueInformationExtensionField
    {
        public UnknownValueInformationExtensionField(byte fieldByte)
            : base(fieldByte)
        {
        }
    }
}