using System;
using System.Threading;
using System.Threading.Tasks;
using IdentityServer4;

namespace IdentityServerHelper.Prompts
{
    public class ClientSecretPrompt
    {
        private readonly WritePrivateKeyPrompt _writePrivateKeyPrompt;
        private int _recursionDepth = 0;

        public ClientSecretPrompt(WritePrivateKeyPrompt writePrivateKeyPrompt)
        {
            _writePrivateKeyPrompt = writePrivateKeyPrompt;
        }

        public async Task PromptAsync(CancellationToken cancellationToken)
        {
            var selectedOption = await PromptSecretTypeAsync(cancellationToken);

            switch (selectedOption)
            {
                case 1:
                    var secret = SecretGenerator.Generate();

                    Console.WriteLine($"Client Secret: {secret}");
                    Console.WriteLine();

                    var hashed256 = secret.Sha256();
                    var hashed512 = secret.Sha512();

                    Console.WriteLine("Your 256-bit hashed value is:");
                    Console.WriteLine(hashed256);
                    Console.WriteLine("Your 512-bit hashed value is:");
                    Console.WriteLine(hashed512);
                    Console.WriteLine();
                    break;
                case 2:
                    var keys = RsaKeyPairGenerator.Generate();

                    Console.WriteLine("Private Key:");
                    Console.WriteLine(keys.PrivateKey);
                    Console.WriteLine("Public Json Web Key:");
                    Console.WriteLine(keys.PublicKey);
                    Console.WriteLine();

                    Console.WriteLine("Enter YES to write to file system:");
                    var writePromptResponse = Console.ReadLine();

                    if (writePromptResponse.Equals("YES", StringComparison.InvariantCultureIgnoreCase))
                    {
                        await _writePrivateKeyPrompt.PromptAsync(keys.PrivateKey, cancellationToken);
                    }
                    break;
            }
        }

        private Task<int> PromptSecretTypeAsync(CancellationToken cancellationToken)
        {
            var result = 0;

            if (!cancellationToken.IsCancellationRequested)
            {
                Console.WriteLine("Select option:");
                Console.WriteLine("1) Generate Shared Secret");
                Console.WriteLine("2) Genearte RSA Key Pair");

                var validOptionSelected = false;

                var selectedOption = Console.ReadLine();
                if (int.TryParse(selectedOption, out result))
                {
                    if (result == 1 || result == 2)
                    {
                        validOptionSelected = true;
                    }
                }

                if (!validOptionSelected)
                {
                    if (_recursionDepth == Constants.MAX_DEPTH)
                    {
                        Environment.Exit(-1);

                        return Task.FromResult(-1);
                    }
                    _recursionDepth++;
                    Console.WriteLine("Invalid option. Please enter either 1 or 2.");
                    return PromptSecretTypeAsync(cancellationToken);
                }
            }

            return Task.FromResult(result);
        }
    }
}
