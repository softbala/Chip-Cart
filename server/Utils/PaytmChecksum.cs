using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System.Collections.Generic;
namespace PcMate.Api.Utils;
public static class PaytmChecksum
{
    public static string GenerateSignature(IDictionary<string, string> parameters, string merchantKey)
    {
        var sorted = parameters.OrderBy(kv => kv.Key).Select(kv => kv.Key + "=" + kv.Value);
        var payload = string.Join("|", sorted);
        var keyBytes = Encoding.UTF8.GetBytes(merchantKey ?? "");
        using (var hmac = new HMACSHA256(keyBytes))
        {
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
            return Convert.ToHexString(hash).ToLower();
        }
    }

    public static bool VerifySignature(IDictionary<string, string> parameters, string merchantKey, string checksum)
    {
        var gen = GenerateSignature(parameters, merchantKey);
        return string.Equals(gen, checksum, System.StringComparison.OrdinalIgnoreCase);
    }
}