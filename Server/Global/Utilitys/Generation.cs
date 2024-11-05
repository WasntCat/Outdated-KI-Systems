using System;

namespace KINetwork.Global.Utilitys
{
    internal class Generation
    {
        internal static string GenerateSessionToken() => Guid.NewGuid().ToString("N");

        // internal static string GenerateSessionToken() => Convert.ToBase64String(Guid.NewGuid().ToByteArray());
    }
}
