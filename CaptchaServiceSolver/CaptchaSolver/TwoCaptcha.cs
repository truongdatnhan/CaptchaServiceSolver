using CaptchaServiceSolver.Models;
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
    public class TwoCaptcha : CaptchaSolver
    {
        public NameValueCollection? QueryParams { get; set; }
        
        public TwoCaptcha(string key) : base(key)
        {
        }

        public override async Task<(string? taskId, string? error)> SendCaptchaAsync()
        {
            if(QueryParams == null)
            {
                return (null, "Please input params !");
            }
            var uriBuilderSend = new UriBuilder(Util.TWO_CAPTCHA_SEND);
            var paramsQuerySend = HttpUtility.ParseQueryString(string.Empty);
            paramsQuerySend.Add(QueryParams);
            uriBuilderSend.Query = paramsQuerySend.ToString();

            var response = await client.GetAsync(uriBuilderSend.Uri.ToString());
            if (!response.IsSuccessStatusCode)
            {
                return (null, "HTTP FAIL !");
            }
            var result = await response.Content.ReadAsStringAsync();

            var json = JObject.Parse(result);
            var status = (string?)json.GetValue("status");
            var taskId = (string?)json.GetValue("request");
            var error_text = (string?)json.GetValue("error_text");

            if (status != "1" || string.IsNullOrEmpty(taskId))
            {
                return (null, error_text);
            }
            return (taskId, null);
        }

        public override async Task<CaptchaResult?> GetCaptchaAnswerAsync(string captchaId)
        {
            var uriBuilderResult = new UriBuilder(Util.TWO_CAPTCHA_RESULT);
            var paramsQueryResult = HttpUtility.ParseQueryString(uriBuilderResult.Query);
            paramsQueryResult.Add("key", Key);
            paramsQueryResult.Add("action", "get");
            paramsQueryResult.Add("id", captchaId);
            paramsQueryResult.Add("json", "1");

            uriBuilderResult.Query = paramsQueryResult.ToString();

            var response2CaptchaResult = await client.GetAsync(uriBuilderResult.Uri.ToString());
            if (!response2CaptchaResult.IsSuccessStatusCode)
            {
                return null;
            }
            var result2CaptchaResult = await response2CaptchaResult.Content.ReadAsStringAsync();

            var json2CaptchaResult = JObject.Parse(result2CaptchaResult);
            var status = (int?)json2CaptchaResult.GetValue("status");
            var request = (string?)json2CaptchaResult.GetValue("request");
            var jsonCookies = (JArray?)json2CaptchaResult.GetValue("json_cookies");
            var errorText = (string?)json2CaptchaResult.GetValue("error_text");

            return new CaptchaResult
            {
                IsReady = !(status == 0 && request == "CAPCHA_NOT_READY"),
                Answer = request,
                ErrorDesc = errorText,
                Cookies = jsonCookies
            };

        }
    }
}
