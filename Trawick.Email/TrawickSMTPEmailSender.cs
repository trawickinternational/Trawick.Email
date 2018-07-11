using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Trawick.Common.Email;
using System.Net.Mail;

namespace Trawick.Email
{
    public class TrawickOffice365EmailSender : Trawick.Common.Interfaces.IEmailSender
    {
        const String HOST = "smtp.office365.com";
        const int PORT = 587;

        public TrawickOffice365EmailSender() { }
        public EmailResponse SendMail(EmailArgs args)
        {

            var UserName = System.Configuration.ConfigurationManager.AppSettings["EmailUser"];
            var PWord = System.Configuration.ConfigurationManager.AppSettings["EmailPassword"];
            var FromAddress = System.Configuration.ConfigurationManager.AppSettings["FromEmailAddress"];
            SmtpClient client = new SmtpClient(HOST, PORT);


            client.Credentials = new System.Net.NetworkCredential(UserName, PWord);
            client.EnableSsl = true;

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(FromAddress);
            mail.Subject = args.EmailSubject;
            mail.IsBodyHtml = args.IsHtml;
            mail.Body = args.EmailBody;

            foreach (var item in args.EmailTo.Split().Select(m => m))
                mail.To.Add(item);

            foreach (var item in args.EmailBCC.Split().Select(m => m))
                mail.Bcc.Add(item);

            foreach (var item in args.EmaillCC.Split().Select(m => m))
                mail.CC.Add(item);


            try
            {
                if (Trawick.Data.Models.EmailRepo.EmailLog_GetByMailId(args.EmailId).Where(m => m.emailAddr == mail.To.FirstOrDefault().Address && m.ErrMessage == "success").Count() <= 0)
                {
                    client.Send(mail);
                }
                return new EmailResponse { Message = "Success", Status = 1 };

            }

            catch (Exception e)
            {
                return new EmailResponse { Message = e.Message, Status = 2 };
            }

        }
    }

    public class TrawickEmailFromDatabaseTableSender : Trawick.Common.Interfaces.IEmailSender
    {

        public TrawickEmailFromDatabaseTableSender() { }
        public EmailResponse SendMail(EmailArgs args)
        {
            if (Trawick.Data.Models.EmailRepo.Email_Cue_Create(args).email_uid > 0)
            {
                return new EmailResponse()
                {
                    Message = "Success",
                    Status = 4
                };
            }
            else
                return new EmailResponse()
                {
                    Message = "Error Creating Mail Object",
                    Status = 0
                };
        }
    }





}

