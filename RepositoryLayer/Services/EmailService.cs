using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace RepositoryLayer.Services
{
    public class EmailService
    {
        public static void SendMail(string Email, string token)
        {
            using (SmtpClient client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = true;
                client.Credentials = new NetworkCredential("787abhi456@gmail.com", "Abhi@1234");

                MailMessage messageObj = new MailMessage();
                messageObj.To.Add(Email);
                messageObj.From = new MailAddress("787abhi456@gmail.com");
                messageObj.Subject = "Password Reset link";
                messageObj.IsBodyHtml = true;
                messageObj.Body = $"<!DOCTYPE html>" +
                                   "<html>" +
                                   "<body style=\"background -color:#ff7f26;text-align:center;\"> " +
                                   "<h1 style=\"color:#051a80;\">Hello Abhinav</h1>" +
                                   "<h2 style=\"color:#800000;\">Please tab on the below link to change yourn password.</h2>" +
                                   "</body>" + $"www.fundooNotes.com/reset-password/{token}" +

                                    "<html>";
                client.Send(messageObj);
            }
        }
    }
}
