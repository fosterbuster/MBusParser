using System;
using MBus;
using MBus.DataRecord.DataRecordHeader.DataInformationBlock;
using MBus.Extensions;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var bytes = "68FFFF6808017267440090361CC7020200000004050000000004FB827500000000042A1500000004FB9772E2FFFFFF04FBB7722500000002FDBA73470284808040FD480000000004FD48ED0200008440FD5900000000848040FD590000000084C040FD59100000001F".HexStringToBytes();

            
            var parser = new MBusParser();

            while (true)
            {
                var header = parser.ParseHeader(bytes);

                var payload = parser.ParsePayload(header, bytes);
            }

        }
    }
}
