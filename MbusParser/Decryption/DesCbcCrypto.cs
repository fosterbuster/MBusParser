using System.Security.Cryptography;

namespace MBus.Extensions.Decryption
{
    /// <summary>
    ///     DES Encrypter/Decrypter
    /// </summary>
    public sealed class DesCbcCrypto : IMbusCrypto
    {
        private const CipherMode CipherMode = System.Security.Cryptography.CipherMode.CBC;
        private const PaddingMode PaddingMode = System.Security.Cryptography.PaddingMode.None;

        private readonly SymmetricAlgorithm _algorithm;

        public DesCbcCrypto( byte[] key, byte[] initializationVector )
        {
            _algorithm = new DESCryptoServiceProvider
            {
                IV = initializationVector,
                Key = key,
                Mode = CipherMode,
                Padding = PaddingMode
            };
        }

        public byte[] Decrypt( byte[] bytes )
        {
            var transform = _algorithm.CreateDecryptor( );
            return transform.TransformFinalBlock( bytes, 0, bytes.Length );
        }

        public byte[] Encrypt( byte[] bytes )
        {
            var transform = _algorithm.CreateEncryptor( );
            return transform.TransformFinalBlock( bytes, 0, bytes.Length );
        }
    }
}