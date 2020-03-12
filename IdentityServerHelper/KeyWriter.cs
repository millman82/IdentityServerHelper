using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityServerHelper
{
    public class KeyWriter
    {
        public async Task WriteAsync(string directoryPath, string keyJson, CancellationToken cancellationToken)
        {
            var filePath = Path.Combine(directoryPath, ".credentials_rsaparams");

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var encryptedBytes = ProtectedData.Protect(Encoding.UTF8.GetBytes(keyJson), null, DataProtectionScope.LocalMachine);
                await File.WriteAllBytesAsync(filePath, encryptedBytes, cancellationToken);
            }
            else
            {
                await File.WriteAllTextAsync(filePath, keyJson, cancellationToken);
            }
        }
    }
}
