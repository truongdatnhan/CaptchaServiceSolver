using CaptchaServiceSolver.Models;
using CaptchaServiceSolver.Requests.AntiCaptcha;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CaptchaServiceSolver.CaptchaSolver
{
    public class AntiCaptcha : CaptchaSolver
    {
        public override async Task<string?> SendCaptchaAsync(RecaptchaChallenge key)
        {
            var taskRequest = new AntiCaptchaTask(key.Type,key.WebsiteUrl,key.Sitekey, key.WebsiteUrl);

            var requestObject = new AntiCaptchaRequest();

            var response = await client.GetAsync(uriBuilderSend.Uri.ToString());
            var result = await response.Content.ReadAsStringAsync();

            var json = JObject.Parse(result);
            var statusSend = (string?)json.GetValue("status");
            var requestSend = (string?)json.GetValue("request");

            if (statusSend != "1" || string.IsNullOrEmpty(requestSend))
            {
                return null;
            }
            return requestSend;
        }

        public static async Task<string?> GetStatementAsync(UserInfo user)
        {
            var getStatementRequestObject = new GetStatementRequest
            {
                clientPubKey = user.ClientPubKey,
                DT = user.DT,
                PM = user.PM,
                OV = user.OV,
                lang = user.Lang,
                accountNo = user.AccountNo,
                accountType = user.AccountType,
                fromDate = DateTime.Now.AddDays(-7).ToString("dd/MM/yyyy"),
                toDate = DateTime.Now.ToString("dd/MM/yyyy"),
                pageIndex = 0,
                lengthInPage = 999999,
                stmtDate = "",
                stmtType = "",
                mid = 14,
                cif = user.CIF,
                user = user.Username,
                mobileId = user.MobileID,
                clientId = user.ClientID,
                sessionId = user.SessionID
            };

            using var getStatementRequest = new HttpRequestMessage(HttpMethod.Post, "/bank-service/v1/transaction-history");

            var getStatementMessage = Util.EncryptRequest(getStatementRequestObject);
            var getStatementContent = new StringContent(JsonConvert.SerializeObject(getStatementMessage), Encoding.UTF8, "application/json");
            getStatementRequest.Headers.Add("Accept-Language", "vi");
            getStatementRequest.Headers.Add("Accept", "application/json");
            getStatementRequest.Headers.Add("X-Channel", "Web");
            getStatementRequest.Headers.Add("X-Request-ID", Util.CreateEpochTime());
            getStatementRequest.Content = getStatementContent;
            var getStatementResponse = await client.SendAsync(getStatementRequest);

            if (!getStatementResponse.IsSuccessStatusCode)
            {
                Console.WriteLine("LoginAsync: HTTP Request returning bad !");
                Log.Fatal("HTTP Request returning bad !");
                return null;
            }

            var getStatementResult = await getStatementResponse.Content.ReadAsStringAsync();
            var response = Util.DecryptResponse(JsonConvert.DeserializeObject<Message>(getStatementResult));
            Log.Information("{@result}", response);
            if (response != null)
            {
                return response;
            }
            return null;
        }

        public override async Task<(bool isReady, string? result, string? errorDesc)> GetCaptchaAnswerAsync(string captchaId)
        {
            var uriBuilderResult = new UriBuilder(Util.CAPTCHA_RESULT);
            var paramsQueryResult = HttpUtility.ParseQueryString(uriBuilderResult.Query);
            paramsQueryResult.Add("key", Util.CAPTCHA_KEY);
            paramsQueryResult.Add("action", "get");
            paramsQueryResult.Add("id", captchaId);
            paramsQueryResult.Add("json", "1");

            uriBuilderResult.Query = paramsQueryResult.ToString();

            var response2CaptchaResult = await client.GetAsync(uriBuilderResult.Uri.ToString());
            var result2CaptchaResult = await response2CaptchaResult.Content.ReadAsStringAsync();

            var json2CaptchaResult = JObject.Parse(result2CaptchaResult);
            var status = (string?)json2CaptchaResult.GetValue("status");
            var request = (string?)json2CaptchaResult.GetValue("request");
            var jsonCookies = (JArray?)json2CaptchaResult.GetValue("json_cookies");

            if (jsonCookies != null && jsonCookies.Count > 0)
            {
                Util.AddCookiesFromJson(handler.CookieContainer, jsonCookies);
            }

            if (json2CaptchaResult.ContainsKey("error_text"))
            {
                var errorText = (string?)json2CaptchaResult.GetValue("error_text");
                return (true, request, errorText);
            }

            if (status == "1")
            {
                return (true, request, null);
            }
            if (status == "0" && request == "CAPCHA_NOT_READY")
            {
                return (false, null, null);
            }
            return (true, null, null);
        }
    }
}
