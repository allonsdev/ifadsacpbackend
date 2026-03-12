using System.Net;
using System.Net.Mail;

public class EmailSender : IEmailSender
{
    public async Task SendEmailAsync(string email, string subject, string message)
    {
        string emailaddress = "sacp2024@gmail.com";
        var client = new SmtpClient("smtp.gmail.com", 587)
        {
            EnableSsl = true,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(emailaddress, "rdegmvctavguiubb")
        };

        var mailMessage = new MailMessage(from: emailaddress, to: email, subject: subject, body: message);
        AlternateView htmlView = AlternateView.CreateAlternateViewFromString(message, null, "text/html");
        mailMessage.AlternateViews.Add(htmlView);

        try
        {
            await client.SendMailAsync(mailMessage);
            Console.WriteLine("Email sent successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send email: {ex.Message}");
        }
    }
}