﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaptchaServiceSolver.Models
{
    public class CaptchaResult
    {
        public bool IsReady { get; set; }
        public int Status { get; set; }
        public string Answer { get; set; } = default!;
        public string? ErrorDesc { get; set; }
        public JArray? Cookies { get; set; }
    }
}