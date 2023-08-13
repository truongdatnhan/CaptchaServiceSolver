using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaptchaServiceSolver.Models
{
    public class RecaptchaChallenge
    {
        public string Gid { get; set; } = default!;
        public string Type { get; set; } = default!;
        public string Sitekey { get; set; } = default!;
        public string DataS { get; set; } = default!;
        public string WebsiteUrl { get; set; } = default!;
    }
}
