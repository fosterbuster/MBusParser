namespace MBus.Extensions.Decryption
{
    public static class MbusCryptoProvider
    {
        /// <summary>
        ///     Returns <paramref name="payload" /> decrypted using AES128-CBC
        /// </summary>
        /// <param name="key">The decryption key</param>
        /// <param name="iv">The initialization vector</param>
        /// <param name="payload">The bytes to decrypt</param>
        /// <returns><paramref name="payload" /> decrypted</returns>
        public static byte[] DecryptAes128Cbc( byte[] key, byte[] iv, byte[] payload )
        {
            return new AesCbcCrypto( key, iv ).Decrypt( payload );
        }

        /// <summary>
        ///     Returns <paramref name="payload" /> decrypted using AES128-CTR
        /// </summary>
        /// <param name="key">The decryption key</param>
        /// <param name="iv">The initialization vector</param>
        /// <param name="payload">The bytes to decrypt</param>
        /// <returns><paramref name="payload" /> decrypted</returns>
        public static byte[] DecryptAes128Ctr( byte[] key, byte[] iv, byte[] payload )
        {
            return new AesCtrCrypto( key, iv ).Decrypt( payload );
        }

        /// <summary>
        ///     Returns <paramref name="payload" /> decrypted using DES-CBC
        /// </summary>
        /// <param name="key">The encryption key</param>
        /// <param name="iv">The initialization vector</param>
        /// <param name="payload">The bytes to decrypt</param>
        /// <returns><paramref name="payload" /> decrypted</returns>
        public static byte[] DecryptDes( byte[] key, byte[] iv, byte[] payload )
        {
            return new DesCbcCrypto( key, iv ).Decrypt( payload );
        }
    }
}