using CaptchaServiceSolver.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CaptchaServiceSolver.CaptchaSolver
{
    public abstract class CaptchaSolver
    {
        protected HttpClient client;
        protected HttpClientHandler handler;
        protected string Key { get; set; }
        protected CaptchaSolver(string key)
        {
            handler = new HttpClientHandler();
            handler.AllowAutoRedirect = true;
            handler.UseCookies = true;
            handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.Brotli;
            client = new HttpClient(handler);
            client.DefaultRequestHeaders.UserAgent.TryParseAdd(Util.USER_AGENT);
            client.DefaultRequestHeaders.Add("Connection", "keep-alive");
            Key = key;
        }

        public abstract Task<(string? taskId, string? error)> SendCaptchaAsync();
        public abstract Task<CaptchaResult?> GetCaptchaAnswerAsync(string captchaId);
    }
}
