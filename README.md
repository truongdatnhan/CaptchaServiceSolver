<h1 align="center">CaptchaServiceSolver</h1>

> A C# library solving captcha through provided service.

## 🚀 Usage

Declare a CaptchaSolver and initialize with your service.
```sh
CaptchaSolver? captchaSolver;

Console.WriteLine("Choose how auto work:");
Console.WriteLine("1) Solve from 2Captcha");
Console.WriteLine("2) Solve from AntiCaptcha");
Console.Write("\r\nSelect an option: ");
var choice = Console.ReadLine();

switch (choice)
{
    case "1":
        captchaSolver = new TwoCaptcha(CaptchaUtil.TWO_CAPTCHA_KEY);
        break;
    case "2":
        captchaSolver = new AntiCaptcha(CaptchaUtil.ANTI_CAPTCHA_KEY);
        break;
    default:
        Console.WriteLine("Please select an option !");
        return;
}
```

Example of sending a request
```sh
switch (solver)
{
  case TwoCaptcha twoCaptcha:
  {
    var queryParams = new NameValueCollection
    {
      { "your data", token },
    };
  twoCaptcha.QueryParams = queryParams;
  }
  break;
  case AntiCaptcha antiCaptcha:
  {
  Dictionary<string, string> paramsBody = new()
  {
    { "your data", token }
  };
  var task = new AntiCaptchaTask("RecaptchaV2EnterpriseTaskProxyless", Util.SIGN_UP, challenge.sitekey, paramsBody);
  var request = new AntiCaptchaRequest(ANTI_CAPTCHA_KEY, task);
  antiCaptcha.Request = request;
  }
  break;
}
var taskReceive = await captchaSolver.SendCaptchaAsync();
```

## 🤝 Contributing

Contributions, issues and feature requests are welcome.<br />
Feel free to check [issues page](https://github.com/truongdatnhan/CaptchaServiceSolver/issues) and make pull request if you want to contribute.<br />

## Author

👤 **TRUONG DAT NHAN (Zeteo)**

- Github: [@truongdatnhan](https://github.com/truongdatnhan)
- LinkedIn: [Nhan Truong Dat](https://www.linkedin.com/in/nhantruongdat/)
- Personal Website: [Store Linh Tinh](https://storelinhtinh.com/)

## 📝 License

Copyright © 2019 [Truong Dat Nhan](https://github.com/truongdatnhan).<br />
This project is [MIT](https://github.com/truongdatnhan/CaptchaServiceSolver/blob/master/LICENSE) licensed.

---

_This README was generated with ❤️ by [readme-md-generator](https://github.com/kefranabg/readme-md-generator)_
