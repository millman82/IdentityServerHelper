using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Security.Cryptography;

namespace IdentityServerHelper
{
    internal static class RsaKeyPairGenerator
    {
        private static readonly JsonSerializerSettings _serializerSettings;
        static RsaKeyPairGenerator()
        {
            _serializerSettings = new JsonSerializerSettings();
            _serializerSettings.ContractResolver = new DefaultContractResolver() { NamingStrategy = new CamelCaseNamingStrategy() };
        }

        public static (string PrivateKey, string PublicKey) Generate()
        {
            var rsa = new RSACryptoServiceProvider(2048);

            var privateKey = rsa.ExportParameters(true);
            var publicKey = rsa.ExportParameters(false);

            var publicRsaSecurityKey = new RsaSecurityKey(publicKey);
            var jwk = JsonWebKeyConverter.ConvertFromRSASecurityKey(publicRsaSecurityKey);

            var privateKeyJson = JsonConvert.SerializeObject(privateKey, _serializerSettings);
            var publicKeyJson = JsonConvert.SerializeObject(jwk, _serializerSettings);

            return (privateKeyJson, publicKeyJson);
        }
    }
}
