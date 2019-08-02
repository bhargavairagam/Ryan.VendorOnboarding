using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace Ryan.VendorOnboarding.WebUI.Infrastructure
{
    public  class EmailNotificationUtility
    {
        public string Vemail { get; set; }
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

            emailMsg.To.Add(this.Vemail);

            emailMsg.Subject = this.Subject;
            emailMsg.Body = this.Body;
            emailMsg.IsBodyHtml = true;

            client.Send(emailMsg);
        }

        public static async Task SendEmailToClient(List<string> vemail, string vendorguid, string VendorDBAName, string emailbody, string emailsubject, string attachpath)
        {

            string SmtpServer1 = ConfigurationManager.AppSettings["smtpserver"].ToString(); ;

            var message = new MailMessage();

            foreach (string s in vemail)
            {
                message.To.Add(s);
            }
            if(vemail.Count > 0)
            {
                message.From = new MailAddress("Vendor.Mgmt@RyanCompanies.com");
                message.Subject = emailsubject;
                if(!string.IsNullOrEmpty(attachpath))
                {
                    message.Attachments.Add(new Attachment(attachpath));
                }
                

                message.Body = emailbody;

                message.IsBodyHtml = true;
                using (var smtpClient = new SmtpClient(SmtpServer1))
                {
                    await smtpClient.SendMailAsync(message);
                }
            }

          
        }

        public static async Task SendEmailToClient(List<string> vemail, string vendorguid, string VendorDBAName, string emailbody, string emailsubject)
        {

            string SmtpServer1 = HelperClass.smtpserver;
            var message = new MailMessage();

            foreach (string s in vemail)
            {
                message.To.Add(s);
            }
            if (vemail.Count > 0)
            {
                message.From = new MailAddress("Vendor.Mgmt@RyanCompanies.com");
                message.Subject = emailsubject;


                message.Body = emailbody;

                message.IsBodyHtml = true;
                using (var smtpClient = new SmtpClient(SmtpServer1))
                {
                    await smtpClient.SendMailAsync(message);
                }
            }


        }


    }
}