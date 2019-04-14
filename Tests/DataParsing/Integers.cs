using MBus;
using MBus.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Tests.DataParsing
{
    public class Integers
    {
        /*
        * MANUFACTURER:    HST
        * SERIAL:          12345678
        * VERSION:         255
        * TYPE:            OTHER
        * STATUS:          0
        * CONF:            NONE
        */
        //                             LLCC MMMM AAAAAAAA VV TT CI AC ST CONF
        readonly string headerBytes = "0000 7422 78563412 FF 00 7A 00 00 0000";

        // Energy with 0 multiplier, no extension
        readonly string vif = "03";
        readonly string rssi = "00";
        readonly MBusParser parser = new MBusParser();


        [Fact]
        public void CanParseEightBitInteger()
        {
            var dif = "01"; // 8 bit
            var data = "01"; // 1

            var result = ParseValue<double>(dif, data);

            Assert.Equal(1, result);
        }

        [Fact]
        public void CanParseSixteenBitInteger()
        {
            var dif = "02"; // 16 bit
            var data = "00 01"; // 1

            var result = ParseValue<double>(dif, data);

            Assert.Equal(1, result);
        }

        [Fact]
        public void CanParseTwentyFourBitInteger()
        {
            var dif = "03"; // 24 bit
            var data = "00 00 01"; // 1

            var result = ParseValue<double>(dif, data);

            Assert.Equal(1, result);
        }

        [Fact]
        public void CanParseThirtyTwoBitInteger()
        {
            var dif = "04"; // 32 bit
            var data = "00 00 00 01"; // 1

            var result = ParseValue<double>(dif, data);

            Assert.Equal(1, result);
        }

        [Fact]
        public void CanParseThirtyTwoBitReal()
        {
            var dif = "05"; // 32 bit
            var data = "00 00 00 01"; // 1

            var result = ParseValue<float>(dif, data);

            Assert.Equal(1, result);
        }

        [Fact]
        public void CanParseFourtyEightBitInteger()
        {
            var dif = "06"; // 48 bit
            var data = "00 00 00 00 00 01"; // 1

            var result = ParseValue<double>(dif, data);

            Assert.Equal(1, result);
        }


        [Fact]
        public void CanParseSixtyFourBitInteger()
        {
            var dif = "07"; // 64 bit
            var data = "00 00 00 00 00 00 00 01"; // 1

            var result = ParseValue<double>(dif, data);

            Assert.Equal(1, result);
        }

        private byte[] GetBytes(string dif, string data)
        {
            return (headerBytes + dif + vif + data + rssi).Replace(" ", "").HexStringToBytes();
        }

        private T ParseValue<T>(string dif, string data)
        {
            var bytes = GetBytes(dif, data);

            var header = parser.ParseHeader(bytes);
            var payload = parser.ParsePayload(header, bytes);

            return (T)payload.Records[0].DataBlock.Value;
        }
    }
}
