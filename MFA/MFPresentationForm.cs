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
    class MFPresentationForm : IAdapterPresentationForm
    {
        string ErrorMessage;
        string UserId;

        public MFPresentationForm(string errorMessage, string userId)
        {
            ErrorMessage = errorMessage;
            UserId = userId;
        }

        public string GetFormHtml(int lcid)
        {
            string htmlTemplate = Resources.SmsLogin;
            if(string.IsNullOrEmpty(ErrorMessage))
            {
                htmlTemplate = htmlTemplate.Replace("[MessageArea]", "");
            }
            else
            {
                htmlTemplate = htmlTemplate.Replace("[MessageArea]", "<label class='block' style='color: yellow'>" + ErrorMessage + "</label>");
            }
            htmlTemplate = htmlTemplate.Replace("[USERID]", UserId);
            return htmlTemplate;
        }
 
        public string GetFormPreRenderHtml(int lcid)
        {
            return null;
        }
        
        public string GetPageTitle(int lcid)
        {
            return "Custom Authentication";
        }
    }
}
