/***********************************
 * Fırat Akcan                     *
 * firat.akcan@ykteknoloji.com.tr  *
 * akcan.firat@gmail.com           *
 *                                 *
 *                      2021, Jan. *
 **********************************/

using System;
using System.Text;
using System.Security.Cryptography;

namespace MFA
{
    public static class Crypto
    {
        public static string Protect(string stringToEncrypt, string optionalEntropy = null, DataProtectionScope scope = DataProtectionScope.CurrentUser)
        {
            return Convert.ToBase64String(
                ProtectedData.Protect(
                    Encoding.UTF8.GetBytes(stringToEncrypt)
                    , optionalEntropy != null ? Encoding.UTF8.GetBytes(optionalEntropy) : null
                    , scope));
        }

        public static string Unprotect(string encryptedString, string optionalEntropy = null, DataProtectionScope scope = DataProtectionScope.CurrentUser)
        {
            return Encoding.UTF8.GetString(
                ProtectedData.Unprotect(
                    Convert.FromBase64String(encryptedString)
                    , optionalEntropy != null ? Encoding.UTF8.GetBytes(optionalEntropy) : null
                    , scope));
        }
    }
}