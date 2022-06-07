using Microsoft.AspNetCore.Mvc;

namespace NewListWizard.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly AuthService authenticationService;
        public AuthenticationController(AuthService authenticationService)
        {
            this.authenticationService = authenticationService;
        }
        public IActionResult Index()
        {
            return View();
        }
        
        
        public IActionResult Register()
        {
            return View(new UserInfo());
        }

        [HttpPost]
        public async Task<IActionResult>Register(UserInfo registerUser)
        {
            if (ModelState.IsValid)
            {
                var result = await authenticationService.RegisterUserAsync(registerUser);
            }
            return View();

        }


        public IActionResult LoginPage()
        {
            Login login = new Login();
            if(Request.Cookies.Count() > 3)
            {
                login.Email = Request.Cookies["Email"];
                login.Password = Request.Cookies["Password"];
                login.RememberMe = true;
                return RedirectToAction("Login", login);
            }
           
            return View(login);
        }
        //[HttpPost]
        public async Task<IActionResult> Login(Login loginUser)
        {
            if (ModelState.IsValid)
            {

                ViewBag.response = await authenticationService.LoginUserAsync(loginUser);
                return RedirectToAction("Index","Wizard");
              
            }
            return View(loginUser);

        }

        //[HttpPost]
        public async Task<IActionResult> Logout()
        {
            await authenticationService.Logout();
            return RedirectToAction("LoginPage");
        }


        public ViewResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotPassword)
        {
            if (ModelState.IsValid)
            {
                var emailResult =await authenticationService.ForgotPasswordAsync(forgotPassword);
                if(emailResult == "ValidUser")
                {
                    HttpContext.Session.SetString("ResetPasswordMailId", forgotPassword.Email);
                    return View("MailSimulation");
                    //return RedirectToAction("ResetPassword");
                }
                else
                {
                    ViewBag.Message = "Invalid Email";
                    return View(forgotPassword);
                }
                

            }
            else
            { return View(); }
        }

        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPassword)
        {
            if (ModelState.IsValid)
            {
                await authenticationService.ResetPasswordAsync(resetPassword.Password);
            }
            return View();
        }


    }


}
