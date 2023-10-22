using PHKAPI.Models;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace PHKAPI.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public void SendLoginEmail(EmailDBModel emailModel, string otpCode)
        {
            //string otpCode = GenerateRandomOTP();
            var smtpSettings = _config.GetSection("SmtpLoginSettings");
            var smtpClient = new SmtpClient(smtpSettings["SmtpServer"])
            {
                Port = Convert.ToInt32(smtpSettings["SmtpPort"]),
                Credentials = new NetworkCredential(smtpSettings["SmtpUsername"], smtpSettings["SmtpPassword"]),
                EnableSsl = false, // Use SSL if required by your SMTP server
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(smtpSettings["SmtpUsername"]),
                Subject = "OTP for Login",
                Body = GenerateOTPEmailBody(otpCode),
                IsBodyHtml = true
            };

            mailMessage.To.Add(emailModel.To);

            smtpClient.Send(mailMessage);
        }
        public void SendBookingEmail(EmailDBModel emailModel)
        {
            var smtpSettings = _config.GetSection("SmtpSettings");
            var smtpClient = new SmtpClient(smtpSettings["SmtpServer"])
            {
                Port = Convert.ToInt32(smtpSettings["SmtpPort"]),
                Credentials = new NetworkCredential(smtpSettings["SmtpUsername"], smtpSettings["SmtpPassword"]),
                EnableSsl = true, // Use SSL if required by your SMTP server
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(smtpSettings["SmtpUsername"]),
                Subject = emailModel.Subject,
                Body = emailModel.Body,
                IsBodyHtml = true // Set to true if you're sending HTML emails
            };

            mailMessage.To.Add(emailModel.To);

            smtpClient.Send(mailMessage);
        }

        static string GenerateOTPEmailBody(string otpCode)
        {
            // Define the HTML email body
            StringBuilder emailBody = new StringBuilder();
            emailBody.AppendLine("<!DOCTYPE html>");
            emailBody.AppendLine("<html>");
            emailBody.AppendLine("<head>");
            emailBody.AppendLine("<meta charset=\"UTF-8\">");
            emailBody.AppendLine("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
            emailBody.AppendLine("<title>OTP Verification</title>");
            emailBody.AppendLine("</head>");
            emailBody.AppendLine("<body>");
            emailBody.AppendLine("<div style=\"max-width: 600px; margin: 0 auto; padding: 20px;\">");
            emailBody.AppendLine("<div style=\"background-color: #007BFF; color: white; text-align: center; padding: 10px;\">");
            emailBody.AppendLine("<h1>OTP Verification</h1>");
            emailBody.AppendLine("</div>");
            emailBody.AppendLine("<div style=\"padding: 20px; background-color: #f7f7f7;\">");
            emailBody.AppendLine("<p>Hello,</p>");
            emailBody.AppendLine("<p>Your One-Time Password (OTP) for verification is:</p>");
            emailBody.AppendLine("<p style=\"font-size: 24px; font-weight: bold; text-align: center; margin-top: 20px;\">" + otpCode + "</p>");
            emailBody.AppendLine("<p style=\"font-size: 14px; text-align: center; margin-top: 10px;\">Please use this OTP to complete your verification process. It will expire in a short time for security reasons.</p>");
            emailBody.AppendLine("</div>");
            emailBody.AppendLine("</div>");
            emailBody.AppendLine("</body>");
            emailBody.AppendLine("</html>");

            return emailBody.ToString();
        }

        //static string GenerateRandomOTP()
        //{
        //    // Replace this with your OTP generation logic (e.g., generating a random 6-digit code)
        //    Random random = new Random();
        //    int otp = random.Next(1000, 10000);
        //    return otp.ToString();
        //}
    }
}
