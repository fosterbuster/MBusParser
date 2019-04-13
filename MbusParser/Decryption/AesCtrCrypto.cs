using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;

namespace MBus.Extensions.Decryption
{
    /// <summary>
    ///     AES-128 CTR Encrypte/Decrypter
    /// </summary>
    public sealed class AesCtrCrypto : IMbusCrypto
    {
        private readonly byte[] _iv;
        private readonly byte[] _key;
        private readonly SicBlockCipher mode;

        public AesCtrCrypto( byte[] key, byte[] initializationVector )
        {
            mode = new SicBlockCipher( new AesFastEngine( ) );
            _key = key;
            _iv = initializationVector;
        }


        public byte[] Decrypt( byte[] bytes )
        {
            return BouncyCastleCrypto( forEncrypt: false, bytes, _key, _iv );
        }

        public byte[] Encrypt( byte[] bytes )
        {
            return BouncyCastleCrypto( forEncrypt: true, bytes, _key, _iv );
        }


        private byte[] BouncyCastleCrypto( bool forEncrypt, byte[] input, byte[] key, byte[] iv )
        {
            mode.Init( forEncrypt, new ParametersWithIV( new KeyParameter( key ), iv ) );

            BufferedBlockCipher cipher = new BufferedBlockCipher( mode );

            return cipher.DoFinal( input );
        }
    }
}