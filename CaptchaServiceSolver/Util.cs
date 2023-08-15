using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaptchaServiceSolver
{
    public static class Util
    {
        //2CAPTCHA
        public const string TWO_CAPTCHA_SEND = "http://2captcha.com/in.php";
        public const string TWO_CAPTCHA_RESULT = "http://2captcha.com/res.php";

        //ANTI_CAPTCHA
        public const string ANTI_CAPTCHA_SEND = "https://api.anti-captcha.com/createTask";
        public const string ANTI_CAPTCHA_RESULT = "https://api.anti-captcha.com/getTaskResult";

        //CAPTCHA69
        public const string CAPTCHA69_SEND = "https://captcha69.com/in.php";
        public const string CAPTCHA69_RESULT = "https://captcha69.com/res.php";

        public const string USER_AGENT = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:109.0) Gecko/20100101 Firefox/113.0 ";

    }
}
