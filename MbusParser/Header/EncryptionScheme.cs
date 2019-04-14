using System;
using System.Collections.Generic;
using System.Text;

namespace MBus.Header
{
    /// <summary>
    /// Defines which encryption shceme is used for the data.
    /// </summary>
    public enum EncryptionScheme
    {
        /// <summary>
        /// Unknown encryption scheme. If you see this, the parser needs to be updated.
        /// </summary>
        Unknown = -1,

        /// <summary>
        /// The data is not encrypted.
        /// </summary>
        NoEncryption = 0,

        /// <summary>
        /// The data is encrypted with AES Cipher Block Chaining.
        /// </summary>
        AesCbc = 1,

        /// <summary>
        /// The data is encrypted with AES Counter Mode.
        /// </summary>
        AesCtr = 2,

        /// <summary>
        /// The data is encrypted with DES Cipher Block Chaining, with an initialization vector of 0.
        /// </summary>
        DesCbcIvIsZero = 3,

        /// <summary>
        /// The data is encrypted with DES Cipher Block Chaining.
        /// </summary>
        DesCbc = 4,
    }
}