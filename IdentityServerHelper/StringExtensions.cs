using System.Diagnostics;

namespace IdentityServer4
{
    public static class StringExtensions
    {
        [DebuggerStepThrough]
        public static bool IsMissing(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }
    }
}
