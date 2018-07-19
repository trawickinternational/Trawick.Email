using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Trawick.Common.Email;
using System.Net;
using Trawick.Data.Models;

namespace Trawick.Email.EmailHelpers
{
    public class Enrollment
    {
        private int m_EnrollmentId;
        public Enrollment(int MasterEnrollmentId)
        {
            m_EnrollmentId = MasterEnrollmentId;
        }
        public EmailResponse SendEnrollmentReceipt(int tranType,bool isTest)
        {
            var response = new Trawick.Common.Email.EmailResponse() { };
            try
            {
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
                            EmailSubject = tranType == 1 ? "Insurance Purchase Notification for Member " : "Renewal Receipt for Member ",
                            MasterEnrollmentId = m_EnrollmentId,
                            MemberId = Enroll.member_id,
                            EmailTo = Enroll.email1,
                            IsTest = isTest
                        };

                        args.EmailSubject = args.EmailSubject + Enroll.userid.ToString();

                        //Add BCC
                        if (!string.IsNullOrEmpty(Agent.admin_email) && ((tranType == 1 && Agent.copy_on_enrollment_email) || (tranType == 2 && Agent.copy_on_renewal_emails.GetValueOrDefault())))
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
            }
            catch(Exception e)
            {
                return new EmailResponse() { Message = "Error Creating Email_Cue Record for Enrollment", Status = 99 };
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

    public class ReceiptHelper
    {
        public static int SendFailedReceipts()
        {
            var failed = EmailRepo.Email_EnrollmentFailure_GetAll();

            foreach (var item in failed)
            {
                var e = new Enrollment(item.Master_enrollment_id.GetValueOrDefault());
                var response = e.SendEnrollmentReceipt(item.Type.GetValueOrDefault(), false);

                if (response.Message =="Success")
                {
                    EmailRepo.Emaill_EnrollmentFailure_Delete(item.ID);
                }

            }
            return 1;
        }
    }
}