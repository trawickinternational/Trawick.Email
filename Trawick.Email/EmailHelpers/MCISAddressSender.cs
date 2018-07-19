using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trawick.Email.EmailHelpers
{
    public class MCISAddressSender
    {
        public static Trawick.Common.Email.EmailResponse SendMCISAddress(string EmailBody)
        {
            var response = new Trawick.Common.Email.EmailResponse() { };

            var args = new Trawick.Common.Email.EmailArgs()
            {
                EmailBody = EmailBody,
                EmailSubject = "'Addresses for " + DateTime.Now.AddDays(-1).ToShortDateString() + "',",
                IsHtml = true,
                EmaillCC = "sdobbins@trawickinternational.com",
                EmailTo = "contact@multichoiceinsurance.com",
                IsTest = false
            };

            response = Trawick.Common.Email.EmailFactory.GetEmailFactory().SendMail(args);
            return response;
        }
    }
}