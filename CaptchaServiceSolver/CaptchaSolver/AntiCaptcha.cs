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
        public AntiCaptchaRequest Request { get; set; }

        public AntiCaptcha(AntiCaptchaRequest request) : base()
        {
            this.Request = request;
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
            var taskId = (string)json.GetValue("request")!;
            var error = (string)json.GetValue("errorCode")!;

            if (status > 0 || string.IsNullOrEmpty(taskId))
            {
                return (null, error);
            }
            return (taskId, null);
        }

        public override async Task<CaptchaResult?> GetCaptchaAnswerAsync(string captchaId)
        {

            var jsonObject = new JObject
            {
                {"clientKey", Util.ANTI_CAPTCHA_KEY},
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

            var captchaRes = JsonConvert.DeserializeObject<AntiCaptchaResponse>(result);

            if (captchaRes == null)
            {
                return null;
            }

            if (captchaRes.ErrorId > 0)
            {
                return new CaptchaResult
                {
                    ErrorDesc = captchaRes.ErrorCode
                };
            }

            if (captchaRes.Status == "processing")
            {
                return new CaptchaResult
                {
                    IsReady = false
                };
            }

            //READY
            return new CaptchaResult
            {
                IsReady = true,
                Answer = captchaRes.Solution.GRecaptchaResponse
            };
        }
    }
}
