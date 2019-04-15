using System;
using System.Collections.Generic;
using System.Text;

namespace MBus.DataRecord.DataRecordHeader.ValueInformationBlock.Extension
{
    public enum OrthogonalValueInformationExtension
    {
        MultiplicativeCorrectionFactorMinusSix = 0x77,
        MultiplicativeCorrectionFactorThree = 0x7D,
        AdditiveCorrectionConstant = 0x7B,
    }
}