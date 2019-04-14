using MBus.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MBus.Header
{
    public class Configuration
    {
        private readonly byte[] _bytes;

        public Configuration(byte[] bytes)
        {
            _bytes = bytes ?? throw new ArgumentNullException(nameof(bytes));
        }

        public byte NumberOfEncryptedBlocks => GetNumberOfEncryptedBlocks(_bytes[0]);

        public EncryptionScheme EncryptionScheme => GetEncryptionScheme(_bytes[1]);

        private byte GetNumberOfEncryptedBlocks(byte b)
        {
            switch (EncryptionScheme)
            {
                case EncryptionScheme.DesCbcIvIsZero:
                case EncryptionScheme.DesCbc:
                    return b;
                case EncryptionScheme.AesCbc:
                    return b.GetHighNibble();
                default:
                    return 0;
            }
        }

        private EncryptionScheme GetEncryptionScheme(byte encryptionSchemeByte)
        {
            var enc = encryptionSchemeByte.GetLowNibble();

            switch (enc)
            {
                case 0b0000_0000: return EncryptionScheme.NoEncryption;
                case 0b0000_0001: return EncryptionScheme.AesCtr;
                case 0b0000_0011: return EncryptionScheme.DesCbcIvIsZero;
                case 0b0000_0100: return EncryptionScheme.DesCbc;
                case 0b0000_0101: return EncryptionScheme.AesCbc;
            }

            return EncryptionScheme.Unknown;
        }
    }
}