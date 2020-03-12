using System;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityServerHelper.Prompts
{
    public class CreateClientIdPrompt
    {
        private int _recursionDepth = 0;

        public async Task PromptAsync(CancellationToken cancellationToken)
        {
            var createClientId = await PromptCreateClientIdAsync(cancellationToken);
            if (createClientId)
            {
                Console.WriteLine($"Client Id: {Guid.NewGuid().ToString("n")}");
                Console.WriteLine();
            }
        }

        private Task<bool> PromptCreateClientIdAsync(CancellationToken cancellationToken)
        {
            var result = false;

            if (!cancellationToken.IsCancellationRequested)
            {
                Console.WriteLine("Generate a new Client Id? (Y/n)");

                var newClientIdResponse = Console.ReadLine();
                if (newClientIdResponse.Equals("Y", StringComparison.InvariantCultureIgnoreCase))
                {
                    result = true;
                }
                else if (!newClientIdResponse.Equals("n", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (_recursionDepth == Constants.MAX_DEPTH)
                    {
                        Environment.Exit(-1);

                        return Task.FromResult(false);
                    }
                    _recursionDepth++;
                    Console.WriteLine("Invalid option. Please enter Y for Yes or n for No");
                    return PromptCreateClientIdAsync(cancellationToken);
                }

            }
            return Task.FromResult(result);
        }
    }
}
