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
using System.Web.Script.Serialization;
using MFA.Models;

namespace MFA
{
    public class ServiceHelper
    {
        public string SendUrl { get; set; }
        public string CheckUrl { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public ServiceHelper(string SendUrl, string CheckUrl, string UserName, string Password)
        {
            this.SendUrl = SendUrl;
            this.CheckUrl = CheckUrl;
            this.UserName = UserName;
            this.Password = Password;
        }

        public SmsResponse SendSms(string domainUser, string ipAddress)
        {
            SmsRequest request = new SmsRequest();
            request.ClientIp = ipAddress;
            request.UserName = UserName;
            request.Password = Password;
            request.RegisteredNumber = GetUser(domainUser);
            return ExecuteRequest(SendUrl, request);
        }

        public SmsResponse CheckSms(string domainUser, string ipAddress, string sms)
        {
            SmsRequest request = new SmsRequest();
            request.ClientIp = ipAddress;
            request.UserName = UserName;
            request.Password = Password;
            request.RegisteredNumber = GetUser(domainUser);
            request.SmsText = sms;
            return ExecuteRequest(CheckUrl, request);
        }

        private SmsResponse ExecuteRequest(string url, SmsRequest requestDto)
        {
            SmsResponse responseObj = new SmsResponse();
            try
            { 
                string data = new JavaScriptSerializer().Serialize(requestDto);
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Method = "POST";
                webRequest.ContentType = "application/json";
                webRequest.ContentLength = data.Length;
                using (Stream webStream = webRequest.GetRequestStream())
                using (StreamWriter requestWriter = new StreamWriter(webStream, System.Text.Encoding.ASCII))
                {
                    requestWriter.Write(data);
                }
                WebResponse webResponse = webRequest.GetResponse();
                using (Stream webStream = webResponse.GetResponseStream() ?? Stream.Null)
                using (StreamReader responseReader = new StreamReader(webStream))
                {
                    string response = responseReader.ReadToEnd();
                    responseObj = new JavaScriptSerializer().Deserialize<SmsResponse>(response);
                }
            }
            catch (Exception e)
            {
                responseObj.Message = e.ToString();
                responseObj.Code = -1;
            }
            return responseObj;
        }

        private string GetUser(string domainUser)
        {
            string userCode = domainUser;
            try
            {
                if (userCode.Contains("@"))
                {
                    userCode = userCode.Split('@')[0];
                }
                if(userCode.Contains("\\"))
                {
                    userCode = userCode.Split('\\')[1];
                }
                if (userCode.Contains("/"))
                {
                    userCode = userCode.Split('/')[1];
                }
            }
            catch{

            }
            return userCode;
        }
    }
}
