using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CaptchaServiceSolver.Requests.AntiCaptcha
{
    public class AntiCaptchaRequest
    {
        [JsonProperty("clientKey")]
        public string ClientKey { get; set; }

        [JsonProperty("task")]
        public AntiCaptchaTask Task { get; set; }

        public AntiCaptchaRequest(string clientKey, AntiCaptchaTask task)
        {
            ClientKey = clientKey;
            Task = task;
        }
    }

    public class AntiCaptchaTask
    {

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("websiteURL")]
        public string WebsiteURL { get; set; }

        [JsonPropertyName("websiteKey")]
        public string WebsiteKey { get; set; }

        [JsonPropertyName("enterprisePayload")]
        public JObject EnterprisePayload { get; set; }

        public AntiCaptchaTask(string type, string websiteURL, string websiteKey, Dictionary<string, string> enterprisePayload)
        {
            Type = type;
            WebsiteURL = websiteURL;
            WebsiteKey = websiteKey;
            EnterprisePayload = JObject.FromObject(enterprisePayload);
        }
    }
}
