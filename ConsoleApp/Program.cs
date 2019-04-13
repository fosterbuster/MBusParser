using System;
using MBus;
using MBus.DataRecord.DataRecordHeader.DataInformationBlock;
using MBus.Extensions;
using MBus.Header.ELL;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var bytes = "4F442D2C029762691C048D20F941AED921C302B8A76E776A4643C8AD75C1D5CF1D33B4E4D7A8F2FFFED1F4BCF461599B9BA90720771B72ABC2D44187C25E43EEFEDE19F7F5C15B7607E25FE04CABA076AA".HexStringToBytes();
            var key = "F202CCBA6B72E797740E9692ADDA2B2D".HexStringToBytes();
            var ss = new SessionNumberField("3186C522".HexStringToBytes());
            var test = ss.EncryptionScheme;
            var parser = new MBusParser();

            while (true)
            {
                var header = parser.ParseHeader(bytes);

                var payload = parser.ParsePayload(header, bytes, key);
            }

            /*

            68 FF FF 68 08 01 72 ----
            0  1  2  3  4  5  6  7

            08 01 72 ----
            0  1  2  3
            */

        }
    }
}
