using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Trawick.Common.Email;

namespace Trawick.Email.EmailHelpers
{
    public class PortalContact
    {
        public static Trawick.Common.Email.EmailResponse SendPortalContact(EmailArgs args)
        {
            var response = new Trawick.Common.Email.EmailResponse() { };
            response = Trawick.Common.Email.EmailFactory.GetEmailFactory().SendMail(args);
            return response;
        }
    }
}