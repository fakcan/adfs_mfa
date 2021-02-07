/***********************************
 * Fırat Akcan                     *
 * firat.akcan@ykteknoloji.com.tr  *
 * akcan.firat@gmail.com           *
 *                                 *
 *                      2021, Jan. *
 **********************************/

using Microsoft.IdentityServer.Web.Authentication.External;

namespace MFA
{
    class MFNoSMSForm : IAdapterPresentationForm
    {
        string UserId;

        public MFNoSMSForm(string userId)
        {
            UserId = userId;
        }

        public string GetFormHtml(int lcid)
        {
            string htmlTemplate = Resources.NoSMSLogin;
            htmlTemplate = htmlTemplate.Replace("[USERID]", UserId);
            return htmlTemplate;
        }
 
        public string GetFormPreRenderHtml(int lcid)
        {
            return null;
        }
        
        public string GetPageTitle(int lcid)
        {
            return "YKB Authentication";
        }


    }
}
