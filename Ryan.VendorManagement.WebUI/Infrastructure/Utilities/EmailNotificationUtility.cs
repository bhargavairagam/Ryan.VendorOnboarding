using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Ryan.VendorManagement.WebUI.Infrastructure.Utilities
{
    public class EmailNotificationUtility
    {
        public string vemail { get; set; }
        public string SmtpServer { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string From { get; set; }

        public  void SendNotification()
        {
            //var basicCredential = new NetworkCredential("", "");

            SmtpClient client = new SmtpClient(SmtpServer);
           // client.UseDefaultCredentials = false;
            //client.Credentials = basicCredential;
            MailMessage emailMsg = new MailMessage();
            
            emailMsg.From = new MailAddress(this.From);

            emailMsg.To.Add(this.vemail);

            emailMsg.Subject = this.Subject;
            emailMsg.Body = this.Body;
            emailMsg.IsBodyHtml = true;

            client.Send(emailMsg);
        }
    }
}