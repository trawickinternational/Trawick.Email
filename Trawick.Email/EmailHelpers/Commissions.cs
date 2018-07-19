using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Trawick.Common.Email;

namespace Trawick.Email.EmailHelpers
{
    public class Commissions
    {

        public static EmailResponse SendCommissionsEmail(int AgentId)
        {

            var model = Trawick.Data.Models.AgentRepo.Agent_CommissionEmailAndKey(AgentId);
            if (model != null)
            {

                string body = new System.IO.StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/CommissionsEmail.html")).ReadToEnd();
                body = body.Replace("%agent_key%", model.GUIDStr);


                var args = new EmailArgs()
                {
                    EmailTo = model.commEmail,
                    EmaillCC = "info@trawickinternational.com,sdobbins@trawickinternational.com,sturner@trawickinternational.com",
                    EmailSubject = "Commission Reports for Previous Month",
                    IsHtml = true,
                    EmailBody = body

                };

                var Mailer = Trawick.Common.Email.EmailFactory.GetEmailFactory();

                return Mailer.SendMail(args);
            }

            return new EmailResponse() { Message = "Error Sending Commission Email" };
        }
    }
}