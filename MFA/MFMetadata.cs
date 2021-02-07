/***********************************
 * Fırat Akcan                     *
 * firat.akcan@ykteknoloji.com.tr  *
 * akcan.firat@gmail.com           *
 *                                 *
 *                      2021, Jan. *
 **********************************/

using System.Collections.Generic;
using System.Globalization;
using Microsoft.IdentityServer.Web.Authentication.External;

namespace MFA
{
    class MFMetadata : IAuthenticationAdapterMetadata
    {
        public string AdminName
        {
            get { return "ADFS MFA Authentication v2"; }
        }
        
        public virtual string[] AuthenticationMethods
        {
            get { return new[] { "http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/hardwaretoken" }; }
        }
        
        public int[] AvailableLcids
        {
            get
            {
                return new[] { new CultureInfo("en-us").LCID, new CultureInfo("tr-TR").LCID };
            }
        }
        
        public Dictionary<int, string> FriendlyNames
        {
            get
            {
                Dictionary<int, string> _friendlyNames = new Dictionary<int, string>();
                _friendlyNames.Add(new CultureInfo("en-us").LCID, "Custom Authentication");
                _friendlyNames.Add(new CultureInfo("tr-TR").LCID, "Özel Kimlik Doğrulama");
                return _friendlyNames;
            }
        }
        
        public Dictionary<int, string> Descriptions
        {
            get
            {
                Dictionary<int, string> _descriptions = new Dictionary<int, string>();
                _descriptions.Add(new CultureInfo("en-us").LCID, "Custom Authentication");
                _descriptions.Add(new CultureInfo("tr-TR").LCID, "Özel Kimlik Doğrulama");
                return _descriptions;
            }
        }
        
        public string[] IdentityClaims
        {
            get { return new[] { "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn" }; }
        }
        
        public bool RequiresIdentity
        {
            get { return true; }
        }
    }
}
