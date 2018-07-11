using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Trawick.Common.Email;
using System.Net;

namespace Trawick.Email.EmailHelpers
{
    public class Enrollment
    {
        private int m_EnrollmentId;
        public Enrollment(int MasterEnrollmentId)
        {
            m_EnrollmentId = MasterEnrollmentId;
        }
        public EmailResponse SendEnrollmentReceipt()
        {
            var response = new Trawick.Common.Email.EmailResponse() { };
            var receiptHtml = GetReceiptString();

            if (!string.IsNullOrEmpty(receiptHtml))
            {
                var Enroll = Trawick.Data.Models.EnrollmentRepo.GetById(m_EnrollmentId).Where(m => m.member_relationship_id == 8).FirstOrDefault();

                if (Enroll != null)
                {
                    var Agent = Trawick.Data.Models.ContactRepo.Contact_GetById(Enroll.agent_id);

                    var args = new EmailArgs()
                    {

                        EmailBody = receiptHtml,
                        IsHtml = true,
                        EmailSubject = "Insurance Purchase Notification for Member",
                        MasterEnrollmentId = m_EnrollmentId,
                        MemberId = Enroll.member_id,
                        EmailTo = Enroll.email1
                    };

                    //Add BCC
                    if (Agent.copy_on_enrollment_email && !string.IsNullOrEmpty(Agent.admin_email))
                    {
                        args.EmailBCC = Agent.admin_email;
                    }

                    var otherBcc = System.Configuration.ConfigurationManager.AppSettings["MailSender.Bcc"];
                    if (!string.IsNullOrEmpty(otherBcc))
                    {
                        args.EmailBCC += "," + otherBcc;
                    }

                    var Mailer = Trawick.Common.Email.EmailFactory.GetEmailFactory();

                    return Mailer.SendMail(args);
                }
            }

            return new EmailResponse() { Message = "Error Creating Email_Cue Record for Enrollment", Status = 99 };
        }



        private string GetReceiptString()
        {
            try

            {
                var html = string.Empty;

                WebClient client = new WebClient();
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                string rec = "";
                var pdfUrl = System.Configuration.ConfigurationManager.AppSettings["PDFServiceUrl"];
                html = client.DownloadString(pdfUrl + "/RetrieveReceipt?EnrollID=" + m_EnrollmentId.ToString() + "&newCert=true");
                return html;
            }
            catch (Exception e)
            {
                throw;
              
            }
            return string.Empty;

        }
    }
}