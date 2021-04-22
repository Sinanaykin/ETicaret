using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System;

namespace shopapp.webui.EmailServices
{
    public class SmtpEmailSender:IEmailSender
    { 
        public bool SendEmailAsync(string email,string subject,string htmlMessage)
        {

           
                    
        MailMessage mailMessage = new MailMessage();
        mailMessage.From = new MailAddress("sinanaykinnn@hotmail.com");
        mailMessage.To.Add(new MailAddress(email));

        mailMessage.Subject = subject;
        mailMessage.IsBodyHtml = true;
        mailMessage.Body = htmlMessage;

        SmtpClient client = new SmtpClient();
        client.Credentials = new System.Net.NetworkCredential("sinanaykinnn@hotmail.com", "Shopapp_1234");
        client.Host = "smtp.office365.com";
        client.Port = 587;
        client.EnableSsl = true;
        try
        {
        client.Send(mailMessage);
        return true;
        }
        catch (Exception ex)
        {
            throw ex;
        // log exception
        }
        return false;

         // var client =new SmtpClient("smtp.gmail.com",587)
            // {
            //     Credentials=new NetworkCredential("sinanaykinnn2@gmail.com","Basketciyim.34"),
            //     EnableSsl=true,UseDefaultCredentials=true
            // };
            // return client.SendMailAsync(
            //     new MailMessage("sinanaykinnn2@gmail.com",email,subject,htmlMessage){
            //         IsBodyHtml=true
            //     }
            // );
            // var client =new SmtpClient("smtp.gmail.com",587)
            // {
            //     Credentials=new NetworkCredential("sinanaykinnn2@gmail.com","****"),
            //     EnableSsl=true
            // };
            // return client.SendMailAsync(
            //     new MailMessage("sinanaykinnn2@gmail.com",email,subject,htmlMessage){
            //         IsBodyHtml=true
            //     }
            // );

        }
    }
}
