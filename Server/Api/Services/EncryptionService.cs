using System.Security.Cryptography;
using System.Text;

namespace Api.Services;

public class EncryptionService
{
    private readonly byte[] _key;
    public EncryptionService()
    {
        _key = SHA256.HashData(Encoding.UTF8.GetBytes("test"));
    }

    public string Encrypt(string password)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(password);

        using var aes = Aes.Create();
        aes.Key = _key;
        aes.GenerateIV();

        using var Encryptor = aes.CreateEncryptor();

        using var mEncrypt = new MemoryStream();
        mEncrypt.Write(aes.IV, 0, aes.IV.Length);
        using var cEncrypt = new CryptoStream(mEncrypt, Encryptor, CryptoStreamMode.Write);

        cEncrypt.Write(bytes, 0, bytes.Length);
        cEncrypt.FlushFinalBlock();

        return Convert.ToBase64String(mEncrypt.ToArray());
    }

    public string Decrypt(string password)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(password);

        using var aes = Aes.Create();
        aes.Key = _key;

        byte[] ivBytes = new byte[16];
        Array.Copy(bytes, 0, ivBytes, 0, 16);

        aes.IV = ivBytes;

        using var decryptor = aes.CreateDecryptor();

        using var mDecrypt = new MemoryStream(bytes, 16, bytes.Length - 16);
        using var cDecrypt = new CryptoStream(mDecrypt, decryptor, CryptoStreamMode.Read);
        using var srDecrypt = new StreamReader(cDecrypt);

        return srDecrypt.ReadToEnd();
    }
}