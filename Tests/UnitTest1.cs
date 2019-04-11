using MBus;
using MBus.Extensions;
using System;
using Xunit;

namespace Tests
{
    public class Header
    {
        private MBusParser _parser = new MBusParser();
        private static byte[] _kamstrupPayload = "".HexStringToBytes();
        [Fact]
        public void CanHandleWirelessHeader()
        {
            var header = _parser.ParseHeader(_kamstrupPayload);
            var version = 16;
            Assert.Equal("KAM", header.ManufacturerName);
            Assert.Equal(version, header.VersionNumber);
        }
    }
}
