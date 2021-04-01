using System.Threading.Tasks;

namespace shopapp.webui.EmailServices
{
    public interface IEmailSender
    {
         //smtp=>gmail,hotmail
         //api=>sendgrid

         bool SendEmailAsync(string email,string subject,string htmlMessage);
    }
}