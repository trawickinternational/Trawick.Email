﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Trawick.Common.Email;
using Trawick.Data.Models;

namespace Trawick.Email.EmailHelpers
{
    public class EnrollmentCancellationRequest
    {
        public static Trawick.Common.Email.EmailResponse SendEnrollmentCancellationRequest(Trawick.Data.Models.EnrollmentCancellationRequest request)
        {

            string toEmail = "info@trawickinternational.com";
            string subject = "Enrollment Cancellation Request";
            string message = string.Format("Please Cancel Enrollment {0} on the Following Date: {1}", request.MasterEnrollmentId, request.RequestDate.ToShortDateString());

            string header = "<h2>{0}</h2> ";

            string body = "";

            body += string.Format(header, message);
            body += "<br/>";
            body += request.RequestMessage; 
            string title = string.Format("<h5>{0}</h5>", subject);
            message = body;

            string ccEmail = "";
            if (request.CopyAgent)
            {
                var agent = ContactRepo.Contact_GetById(request.AgentId);
                if (agent != null && !string.IsNullOrEmpty(agent.admin_email))
                {
                    ccEmail = agent.admin_email;
                }
            }

            var args = new Common.Email.EmailArgs()
            {
                EmailBody = message,
                EmaillCC = ccEmail,
                EmailTo = toEmail,
                EmailSubject = subject,
                IsHtml = true
            };

            var response = new Trawick.Common.Email.EmailResponse() { };
            response = Trawick.Common.Email.EmailFactory.GetEmailFactory().SendMail(args);
            return response;

        }

        public static Trawick.Common.Email.EmailResponse SendCancelEnrollmentCancellationRequest(Trawick.Data.Models.EnrollmentCancellationRequest request)
        {

            string toEmail = "info@trawickinternational.com";
            string subject = "Enrollment Cancellation Request";
            string message = string.Format("Please <strong> DO NOT </strong> Cancel Enrollment {0} on the Following Date: {1}", request.MasterEnrollmentId, request.RequestDate.ToShortDateString());

            string header = "<h2>{0}</h2> ";

            string body = "";

            body += string.Format(header, message);
            body += "<br/>";
            body += request.RequestMessage;
            string title = string.Format("<h5>{0}</h5>", subject);
            message = body;

            string ccEmail = "";
            if (request.CopyAgent)
            {
                var agent = ContactRepo.Contact_GetById(request.AgentId);
                if (agent != null && !string.IsNullOrEmpty(agent.admin_email))
                {
                    ccEmail = agent.admin_email;
                }
            }

            var args = new Common.Email.EmailArgs()
            {
                EmailBody = message,
                EmaillCC = ccEmail,
                EmailTo = toEmail,
                EmailSubject = subject,
                IsHtml = true
            };

            var response = new Trawick.Common.Email.EmailResponse() { };
            response = Trawick.Common.Email.EmailFactory.GetEmailFactory().SendMail(args);
            return response;

        }
    }
}