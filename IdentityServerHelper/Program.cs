using System;
using System.Threading;
using System.Threading.Tasks;
using IdentityServerHelper.Prompts;

namespace IdentityServerHelper
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var cancellationTokenSource = new CancellationTokenSource(120000);
            var cancellationToken = cancellationTokenSource.Token;

            var createClientIdPrompt = new CreateClientIdPrompt();
            await createClientIdPrompt.PromptAsync(cancellationToken);

            var keyWriter = new KeyWriter();
            var writePrivateKeyPrompt = new WritePrivateKeyPrompt(keyWriter);
            var clientSecretPrompt = new ClientSecretPrompt(writePrivateKeyPrompt);
            await clientSecretPrompt.PromptAsync(cancellationToken);
        }
    }
}
