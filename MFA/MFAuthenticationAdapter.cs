/***********************************
 * Fırat Akcan                     *
 * firat.akcan@ykteknoloji.com.tr  *
 * akcan.firat@gmail.com           *
 *                                 *
 *                      2021, Jan. *
 **********************************/
using System;
using System.IO;
using System.Net;
using MFA.Models;
using System.Reflection;
using System.Diagnostics;
using System.DirectoryServices;
using Claim = System.Security.Claims.Claim;
using System.DirectoryServices.AccountManagement;
using Microsoft.IdentityServer.Web.Authentication.External;

namespace MFA
{
    public class MFAuthenticationAdapter : IAuthenticationAdapter
    {
        private static Settings settings = new Settings();

        private static bool IsMemberOf(string UserPrincipalName, string Group)
        {
            try
            {
                PrincipalContext domainctx = new PrincipalContext(ContextType.Domain);
                UserPrincipal userPrincipal = UserPrincipal.FindByIdentity(domainctx, IdentityType.UserPrincipalName, UserPrincipalName);
                bool val = userPrincipal.IsMemberOf(domainctx, IdentityType.Name, Group);

                EventLog.WriteEntry("ADFS MFA", string.Format(
                        "Check 1{0}  {1} {2} {3}? {4}",
                        Environment.NewLine, UserPrincipalName, "IsMemberOf", Group, val),
                    EventLogEntryType.Information, 50);

                return val;
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("ADFS MFA", string.Format(
                        "Check 1{0}  Function: {1}{0}  Exception: {2}{0}  Detail: {3}{0}",
                        Environment.NewLine, "IsMemberOf", ex.Message, ex.StackTrace),
                    EventLogEntryType.Error, 55);
            }
            return false;
        }

        private static string GetAttribute(string UserPrincipalName, string AttributeName)
        {
            DirectorySearcher search = new DirectorySearcher(string.Format("(&(ObjectClass=user)(userPrincipalName={0}))", UserPrincipalName), new string[] { AttributeName });
            SearchResult result = search.FindOne();
            if (result != null)
            {
                if (result.Properties.Contains(AttributeName))
                {
                    EventLog.WriteEntry("ADFS MFA", string.Format("Check 2{0}  Function: {1}{0}  User: {2}{0}  AttributeName: {3}{0}  Value: {4}{0}",
                        Environment.NewLine, "GetAttribute", UserPrincipalName, AttributeName, result.Properties[AttributeName][0].ToString()),
                        EventLogEntryType.Information, 70);
                    return result.Properties[AttributeName][0].ToString();
                }
            }
            EventLog.WriteEntry("ADFS MFA", string.Format("Check 2{0}  Function: {1}{0}  User: {2}{0}  AttributeName: {3}{0}  Value: {4}{0}",
                Environment.NewLine, "GetAttribute", UserPrincipalName, AttributeName, "N/A"),
                EventLogEntryType.Information, 75);
            return string.Empty;
        }

        public IAuthenticationAdapterMetadata Metadata
        {
            get { return new MFMetadata(); }
        }

        public IAdapterPresentation BeginAuthentication(Claim identityClaim, HttpListenerRequest request, IAuthenticationContext authContext)
        {
            string userId = identityClaim.Value;
            string userAttributeMFA = GetAttribute(userId, settings.AttributeMFA);

            if (!(IsMemberOf(userId, settings.DisabledMFAGroup) || userAttributeMFA == settings.AttributeMFADisabled))
            {
                ServiceHelper helper = new ServiceHelper(settings.WebServiceSendUrl, settings.WebServiceCheckUrl, settings.WebServiceUser, settings.WebServicePassword);
                if (string.IsNullOrEmpty(userAttributeMFA) || userAttributeMFA == settings.AttributeMFAEnabled)
                {
                    EventLog.WriteEntry(settings.EventLogSource, string.Format(
                            "Step 1{0}  UPN: {1}{0}  ContextID: {2}{0}  Message: {3}{0}",
                            Environment.NewLine, userId, authContext.ContextId, "Calling SMS Web Service"),
                        EventLogEntryType.Information, 100);

                    SmsResponse response = helper.SendSms(userId, request.UserHostAddress);

                    EventLog.WriteEntry(settings.EventLogSource, string.Format(
                            "Step 2{0}  UPN: {1}{0}  ContextID: {2}{0}  Message: {3}{0}  RemoteEndPoint: {4}{0}  Web Service{0}    Response Code: {5}{0}    Response IsError: {6}{0}    Response Message: {7}",
                            Environment.NewLine, userId, authContext.ContextId, "SMS Web Service called", request.RemoteEndPoint, response.Code,
                            response.IsError, response.Message),
                        response.IsError ? EventLogEntryType.Error : EventLogEntryType.Information,
                        response.IsError ? 501 : 200);
                }
                else if (userAttributeMFA == settings.AttributeMFARedirected)
                {
                    var redirectList = GetAttribute(userId, settings.AttributeRedirected).Split(new string[] { settings.AttributeDelimiter }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var rUser in redirectList)
                    {
                        EventLog.WriteEntry(settings.EventLogSource, string.Format(
                            "Step 1{0}  UPN: {1}{0}  ContextID: {2}{0}  Message: {3}{0}",
                            Environment.NewLine, userId, authContext.ContextId, string.Format("Redirected: SMS Web Service will be called for {0} user", redirectList.Length)),
                        EventLogEntryType.Information, 100);

                        if (rUser != string.Empty)
                        {
                            SmsResponse response = helper.SendSms(rUser, request.UserHostAddress);
                            EventLog.WriteEntry(settings.EventLogSource, string.Format(
                                    "Step 2{0}  User: {1}{0}  RedirectedUser: {2}{0}  ContextID: {3}{0}  Message: {4}{0}  RemoteEndPoint: {5}{0}  Web Service{0}    Response Code: {6}{0}    Response IsError: {7}{0}    Response Message: {8}",
                                    Environment.NewLine, userId, rUser, authContext.ContextId, "SMS Web Service called", request.RemoteEndPoint, response.Code,
                                    response.IsError, response.Message),
                                response.IsError ? EventLogEntryType.Error : EventLogEntryType.Information,
                                response.IsError ? 501 : 200);
                        }
                    }
                }
                else
                {
                    return new MFPresentationForm(string.Format("Aktif dizin hesabınızın '{0}' değerini Çözüm Merkezi yardımı ile kontrol ettirin!", settings.AttributeMFA), userId);
                }
                return new MFPresentationForm(string.Empty, userId);
            }
            return new MFNoSMSForm(userId);
        }

        public bool IsAvailableForUser(Claim identityClaim, IAuthenticationContext authContext)
        {
            return true;
        }

        public void OnAuthenticationPipelineLoad(IAuthenticationMethodConfigData configData)
        {
            if (!EventLog.SourceExists(settings.EventLogSource))
            {
                EventSourceCreationData eventSourceData = new EventSourceCreationData(settings.EventLogSource, "Application");
                EventLog.CreateEventSource(eventSourceData);
            }

            if (configData != null)
            {
                if (configData.Data != null)
                {
                    using (StreamReader reader = new StreamReader(configData.Data, System.Text.Encoding.UTF8))
                    {
                        try
                        {
                            Config config = Config.FromXml(reader.ReadToEnd());
                            if (File.Exists(config.Path))
                            {
                                settings = Settings.FromXml(File.ReadAllText(config.Path));

                                PropertyInfo[] properties = typeof(Settings).GetProperties();
                                foreach (PropertyInfo property in properties)
                                {
                                    Encryptable[] attrib = (Encryptable[])property.GetCustomAttributes(typeof(Encryptable), false);
                                    if (attrib.Length != 0)
                                    {
                                        property.SetValue(settings, Crypto.Unprotect(property.GetValue(settings).ToString()));
                                    }
                                }

                                EventLog.WriteEntry(settings.EventLogSource, string.Format("PipelineLoad Config{0}{0}{1}", Environment.NewLine, settings.ToString()), EventLogEntryType.Information, 10);
                            }
                            else
                            {
                                EventLog.WriteEntry(settings.EventLogSource, string.Format("PipelineLoad Config Error: {0}", "Config XML file does not exist"), EventLogEntryType.Error, 14);
                            }
                        }
                        catch (Exception ex){
                            EventLog.WriteEntry(settings.EventLogSource, string.Format("PipelineLoad Config Error: {0}", ex.Message), EventLogEntryType.Error, 15);
                        }
                    }
                }
                else
                {
                    EventLog.WriteEntry(settings.EventLogSource, string.Format("PipelineLoad Config: {0}", "configData.Data is null"), EventLogEntryType.Error, 13);
                }
            }
            else
            {
                EventLog.WriteEntry(settings.EventLogSource, string.Format("PipelineLoad Config: {0}", "configData is null"), EventLogEntryType.Error, 12);
            }
        }

        public void OnAuthenticationPipelineUnload()
        {

        }

        public IAdapterPresentation OnError(HttpListenerRequest request, ExternalAuthenticationException ex)
        {
            EventLog.WriteEntry(settings.EventLogSource, string.Format("Step 3{0}  Error: {1}",
                Environment.NewLine, ex.Message), EventLogEntryType.Error, 500);

            return new MFPresentationForm(ex.ToString(), string.Empty);
        }

        public IAdapterPresentation TryEndAuthentication(IAuthenticationContext authContext, IProofData proofData, HttpListenerRequest request, out Claim[] outgoingClaims)
        {
            outgoingClaims = new Claim[0];
            string userId = proofData.Properties["userId"].ToString();
            string userAttributeMFA = GetAttribute(userId, settings.AttributeMFA);

            if (!(IsMemberOf(userId, settings.DisabledMFAGroup) || userAttributeMFA == settings.AttributeMFADisabled))
            {
                if (proofData == null || proofData.Properties == null || !proofData.Properties.ContainsKey("SMS") || string.IsNullOrEmpty(proofData.Properties["SMS"].ToString()))
                {
                    EventLog.WriteEntry(settings.EventLogSource, string.Format("Step 3{0}  UPN: {1}{0}  ContextID: {2}{0}  Message: {3}",
                            Environment.NewLine, userId, authContext.ContextId, "SMS form was empty"), EventLogEntryType.Warning, 401);

                    return new MFPresentationForm("Lütfen SMS şifresini giriniz!", userId);
                }

                var sms = (string)proofData.Properties["SMS"];
                ServiceHelper helper = new ServiceHelper(settings.WebServiceSendUrl, settings.WebServiceCheckUrl, settings.WebServiceUser, settings.WebServicePassword);
                SmsResponse response = null;

                if (userAttributeMFA == settings.AttributeMFARedirected)
                {
                    var redirectList = GetAttribute(userId, settings.AttributeRedirected).Split(new string[] { settings.AttributeDelimiter }, StringSplitOptions.RemoveEmptyEntries);
                    string verifiedUser = null;
                    foreach (var rUser in redirectList)
                    {
                        if (rUser != string.Empty)
                        {
                            response = helper.CheckSms(rUser, request.UserHostAddress, sms);
                            if (!response.IsError)
                            {
                                verifiedUser = rUser;
                                break;
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(verifiedUser))
                    {
                        EventLog.WriteEntry(settings.EventLogSource, string.Format(
                                    "->{0}  User: {1}{0}  VerifiedByUser: {2}{0}  ContextID: {3}{0}  RemoteEndPoint: {4}{0}  Message: {5}{0}  Web Service{0}    Response Code: {6}{0}    Response IsError: {7}{0}    Response Message: {8}",
                                    Environment.NewLine, userId, verifiedUser, authContext.ContextId, request.RemoteEndPoint, "SMS correct!", response.Code,
                                    response.IsError, response.Message),
                                response.IsError ? EventLogEntryType.Error : EventLogEntryType.Information,
                                response.IsError ? 501 : 210);
                    }
                }
                else
                {
                    response = helper.CheckSms(userId, request.UserHostAddress, sms);
                }

                if (response.IsError)
                {
                    string errMessage = string.Empty;
                    int eventId = 0;
                    switch (response.Message)
                    {
                        case "SmsNotFound":
                            errMessage = "Sms mesajı bulunamadı!";
                            eventId = 404;
                            break;
                        case "WrongSms":
                            errMessage = "Sms mesajı mesajı yanlış, lütfen kontrol edip tekrar giriniz!";
                            eventId = 402;
                            break;
                        case "UnAuthorized":
                            errMessage = "Yetkiniz bulunmamaktadır!";
                            eventId = 403;
                            break;
                        default:
                            errMessage = response.Message;
                            eventId = 502;
                            break;
                    }

                    EventLog.WriteEntry(settings.EventLogSource, string.Format("Step 3{0}  UPN: {1}{0}  ContextID: {2}{0}  RemoteEndPoint: {3}{0}  Message: {4}",
                        Environment.NewLine, userId, authContext.ContextId, request.RemoteEndPoint, response.Message), EventLogEntryType.Error, eventId);

                    return new MFPresentationForm(errMessage, userId);
                }
                else
                {
                    outgoingClaims = new[] {
                        new Claim( "http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod",
                         "http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/hardwaretoken" ) };

                    EventLog.WriteEntry(settings.EventLogSource, string.Format("Step 3{0}  UPN: {1}{0}  ContextID: {2}{0}  RemoteEndPoint: {3}{0}  Message: {4}",
                            Environment.NewLine, userId, authContext.ContextId, request.RemoteEndPoint, "SMS Success"), EventLogEntryType.Information, 220);

                    return null;
                }
            }
            else
            {
                outgoingClaims = new[] {
                    new Claim( "http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod",
                    "http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/hardwaretoken" ) };

                EventLog.WriteEntry(settings.EventLogSource, string.Format("Step 3{0}  UPN: {1}{0}  ContextID: {2}{0}  RemoteEndPoint: {3}{0}  Message: {4}",
                        Environment.NewLine, userId, authContext.ContextId, request.RemoteEndPoint, "User is allowed to login without SMS verification"), EventLogEntryType.Information, 240);

                return null;
            }
        }
    }
}
