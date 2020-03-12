using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityServerHelper.Prompts
{
    public class WritePrivateKeyPrompt
    {
        private readonly KeyWriter _keyWriter;
        private int _recursionDepth = 0;

        public WritePrivateKeyPrompt(KeyWriter keyWriter)
        {
            _keyWriter = keyWriter;
        }

        public async Task PromptAsync(string privateKeyJson, CancellationToken cancellationToken)
        {
            var directoryPath = await PromptWritePrivateKeyAsync(cancellationToken);

            if (!string.IsNullOrEmpty(directoryPath))
            {
                await _keyWriter.WriteAsync(directoryPath, privateKeyJson, cancellationToken);
            }
        }

        private async Task<string> PromptWritePrivateKeyAsync(CancellationToken cancellationToken)
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                Console.WriteLine("Enter directory path:");
                var path = Console.ReadLine();

                if (!Directory.Exists(path))
                {
                    if (_recursionDepth == Constants.MAX_DEPTH)
                    {
                        Console.WriteLine("Unable to write to directory.");

                        Environment.Exit(-1);

                        return null;
                    }

                    Console.WriteLine("Invalid directory. Please try again.");
                    _recursionDepth++;
                    return await PromptWritePrivateKeyAsync(cancellationToken);
                }

                return path;
            }

            return null;
        }
    }
}
