using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaptchaServiceSolver.Responses.AntiCaptcha
{
    public class AntiCaptchaResponse
    {
        [JsonProperty("errorId")]
        public int? ErrorId { get; set; }

        [JsonProperty("errorCode")]
        public string? ErrorCode { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; } = default!;

        [JsonProperty("solution")]
        public Solution Solution { get; set; } = default!;

        [JsonProperty("cost")]
        public string? Cost { get; set; }

        [JsonProperty("ip")]
        public string? Ip { get; set; }

        [JsonProperty("createTime")]
        public int? CreateTime { get; set; }

        [JsonProperty("endTime")]
        public int? EndTime { get; set; }

        [JsonProperty("solveCount")]
        public int? SolveCount { get; set; }
    }
    public class Solution
    {
        [JsonProperty("gRecaptchaResponse")]
        public string GRecaptchaResponse { get; set; } = default!;
    }
}
