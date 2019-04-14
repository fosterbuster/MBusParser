using System.Security.Cryptography;

namespace MBus.Extensions.Decryption
{
    /// <summary>
    ///     AES-128 CBC Encryper/Decrypter
    /// </summary>
    public sealed class AesCbcCrypto : IMbusCrypto
    {
        private const CipherMode CipherMode = System.Security.Cryptography.CipherMode.CBC;
        private const PaddingMode PaddingMode = System.Security.Cryptography.PaddingMode.None;

        private readonly SymmetricAlgorithm _algorithm;

        public AesCbcCrypto( byte[] key, byte[] initializationVector )
        {
            _algorithm = new AesCryptoServiceProvider
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