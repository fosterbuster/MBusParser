using MBus.Extensions;
using System;

namespace MBus.Header.ELL
{
    public class SessionNumberField
    {
        private readonly byte[] _bytes;

        public SessionNumberField(byte[] bytes)
        {
            _bytes = bytes;
        }

        /// <summary>
        /// Gets the encryption scheme.
        /// </summary>
        public EncryptionScheme EncryptionScheme => GetEncryptionScheme(_bytes[3]);

        private EncryptionScheme GetEncryptionScheme(byte encryptionSchemeByte)
        {
            var enc = encryptionSchemeByte.ShiftRight(5).Mask(0b0000_0111);

            switch (enc)
            {
                case 0b0000_0000: return EncryptionScheme.NoEncryption;
                case 0b0000_0001: return EncryptionScheme.AesCtr;
            }

            return EncryptionScheme.Unknown;
        }
    }
}