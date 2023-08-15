using CaptchaServiceSolver.Models;
using CaptchaServiceSolver.Requests.AntiCaptcha;
using CaptchaServiceSolver.Responses.AntiCaptcha;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace CaptchaServiceSolver.CaptchaSolver
{
    public class AntiCaptcha : CaptchaSolver
    {
        public AntiCaptchaRequest? Request { get; set; }

        public AntiCaptcha(string key) : base(key)
        {
        }

        public override async Task<(string? taskId, string? error)> SendCaptchaAsync()
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, Util.ANTI_CAPTCHA_SEND);
            var content = new StringContent(JsonConvert.SerializeObject(Request), Encoding.UTF8, "application/json");
            request.Content = content;
            var responseMessage = await client.SendAsync(request);
            if (!responseMessage.IsSuccessStatusCode)
            {
                return (null, "HTTP FAIL !");
            }
            var result = await responseMessage.Content.ReadAsStringAsync();

            var json = JObject.Parse(result);
            var status = (int?)json.GetValue("errorId");
            var taskId = (string?)json.GetValue("taskId");
            var errorCode = (string?)json.GetValue("errorCode");

            if (status > 0 || string.IsNullOrEmpty(taskId))
            {
                return (null, errorCode);
            }
            return (taskId, null);
        }

        public override async Task<CaptchaResult?> GetCaptchaAnswerAsync(string captchaId)
        {

            var jsonObject = new JObject
            {
                {"clientKey", Key},
                {"taskId", captchaId}
            };

            using var request = new HttpRequestMessage(HttpMethod.Post, Util.ANTI_CAPTCHA_RESULT);
            var content = new StringContent(JsonConvert.SerializeObject(jsonObject), Encoding.UTF8, "application/json");
            request.Content = content;
            var responseMessage = await client.SendAsync(request);
            if (!responseMessage.IsSuccessStatusCode)
            {
                return null;
            }
            var result = await responseMessage.Content.ReadAsStringAsync();
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };
            var captchaRes = JsonConvert.DeserializeObject<AntiCaptchaResponse>(result, settings);

            if (captchaRes == null)
            {
                return null;
            }

            //READY
            return new CaptchaResult
            {
                IsReady = captchaRes.Status == "processing" ? false : true,
                Answer = captchaRes.Status == "ready" ? captchaRes.Solution.GRecaptchaResponse : null,
                ErrorDesc = captchaRes.ErrorId > 0 ? captchaRes.ErrorCode : null
            };
        }
    }
}
