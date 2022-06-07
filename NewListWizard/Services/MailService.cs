namespace NewListWizard.Services
{
    public class MailService
    {
        public void SendMail(string Email)
        {

            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            mail.From = new MailAddress("sender's email");
            mail.To.Add(Email);


            mail.Subject = "ResetPassword";
            mail.Body = $"click on the link" +
            $"https://localhost:7132/Authentication/ResetPassword?email={Email}";


            SmtpServer.Port = 587;
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Credentials = new System.Net.NetworkCredential("sender's email", "password");
            SmtpServer.EnableSsl = true;
            SmtpServer.Send(mail);

        }
    }
}
