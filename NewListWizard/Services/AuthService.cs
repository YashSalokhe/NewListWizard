namespace NewListWizard.Services
{
    public class AuthService
    {
        private readonly NewListWizardContext authenticationContext;
        private readonly EncryptDecryptService passwordEncryptionDecryption;
        private readonly IHttpContextAccessor http;
        private readonly MailService mailService;
        public AuthService(NewListWizardContext authenticationContext, EncryptDecryptService passwordEncryptionDecryption, IHttpContextAccessor http, MailService mailService)
        {
            this.authenticationContext = authenticationContext;
            this.passwordEncryptionDecryption = passwordEncryptionDecryption;
            this.http = http;
            this.mailService = mailService;
        }

        public async Task<string> RegisterUserAsync(UserInfo registerUser)
        {
            try
            {
                var allUsers = await authenticationContext.UserInfos.Select(e=>e.Email).ToListAsync();
                if (!allUsers.Contains(registerUser.Email))
                {
                    registerUser.Password = passwordEncryptionDecryption.Encrypt(registerUser.Password);
                    registerUser.Name = registerUser.Name.ToLower();
                    registerUser.Email = registerUser.Email.ToLower();

                    var result = await authenticationContext.UserInfos.AddAsync(registerUser);
                    await authenticationContext.SaveChangesAsync();
                    return result.ToString(); 
                }
                else
                {
                    return "Email Already Exists";
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
          
        }

        public async Task<string> LoginUserAsync(Login loginUser)
        {
            string response = string.Empty;
            try
            {
                var ValidUser = await authenticationContext.UserInfos.Where(e => e.Email == loginUser.Email && e.IsLockedOut == (byte)isLockedOut.isLockedOutSetToFalse).FirstOrDefaultAsync();
                if(ValidUser != null)
                {
                    string password =  passwordEncryptionDecryption.Decrypt(ValidUser.Password);
                    if (password == loginUser.Password)
                    {
                        ValidUser.LastLoggedIn = DateTime.Now;
                        ValidUser.FailedAttempts = 0;
                        if (loginUser.RememberMe == true)
                        {
                            ValidUser.IsRememberMe = (byte)isRememberMe.isRememberMeSetToTrue;
                            http.HttpContext.Response.Cookies.Append("Email", loginUser.Email);
                            http.HttpContext.Response.Cookies.Append("Password", loginUser.Password);
                            http.HttpContext.Session.SetString("CurrentUserEmail", loginUser.Email);
                        }
                        return response = "success";
                    }
                    else
                    {

                        int count = ValidUser.FailedAttempts++;
                        if(count >= 3)
                        {
                            ValidUser.IsLockedOut = (byte)isLockedOut.isLockedOutSetToTrue;
                            return response = "Your account has been lockedOut";

                        }
                        return response = "Invalid Password";
                    }

                }
                else
                {
                    return response = "Invalid Email";
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                await authenticationContext.SaveChangesAsync();
            }
            
        }

        public async Task Logout()
        {
            var currentUserEmail = http.HttpContext.Session.GetString("CurrentUserEmail");
            if(currentUserEmail != null)
            {
                var user = await authenticationContext.UserInfos.Where(e => e.Email == currentUserEmail).FirstOrDefaultAsync();
                user.IsRememberMe = (byte)isRememberMe.isRememberMeSetToFalse;
                http.HttpContext.Response.Cookies.Delete("Email");
                http.HttpContext.Response.Cookies.Delete("Password");
            }

        }

        public async Task<string> ForgotPasswordAsync(ForgotPasswordViewModel forgotPassword)
        {
            var validUser = await authenticationContext.UserInfos.Where(e=>e.Email == forgotPassword.Email).FirstOrDefaultAsync();

            if (validUser == null)
            {
                return "InvalidUser";
            }
            else
            {
               // mailService.SendMail(forgotPassword.Email);
                return "ValidUser";
            }
        }

        public async Task ResetPasswordAsync(string newPassword)
        {
            var currentUserEmail = http.HttpContext.Session.GetString("ResetPasswordMailId");
            var user = await authenticationContext.UserInfos.Where(e => e.Email == currentUserEmail).FirstOrDefaultAsync();
            var result = passwordEncryptionDecryption.Encrypt(newPassword);
            user.Password = result;
            await authenticationContext.SaveChangesAsync();
        }
    }
}
