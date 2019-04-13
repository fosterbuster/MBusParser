namespace MBus.Extensions.Decryption
{
    public interface IMbusCrypto
    {
        byte[] Decrypt(byte[] bytes);
        byte[] Encrypt(byte[] bytes);
    }
}