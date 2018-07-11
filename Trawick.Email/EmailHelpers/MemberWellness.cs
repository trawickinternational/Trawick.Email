using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trawick.Email.EmailHelpers
{
    public class MemberWellness
    {

        public static Trawick.Common.Email.EmailResponse  SendConfirmationEmail(int WellnessId,string ReceiptStr)
        {
            var response = new Trawick.Common.Email.EmailResponse() { };

            var wellness = Trawick.Data.Models.MemberWellnessRepo.MemberWellness_GetById(WellnessId);

            if (wellness!= null)
            {
                var member = Trawick.Data.Models.MemberRepo.Member_GetById(wellness.member_id);

                var cc = string.Empty;
                if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["MemberWellnessCC"]))
                    cc = System.Configuration.ConfigurationManager.AppSettings["MemberWellnessCC"];

                if (member!= null)
                {
                    var args = new Trawick.Common.Email.EmailArgs()
                    {
                        IsHtml = true,
                        EmailBody = ReceiptStr,
                        EmailSubject = "Member Wellness Receipt",
                        EmailTo = member.email1,
                        MemberId = member.member_id,
                        EmailBCC = cc

                    };

                    Trawick.Common.Email.EmailFactory.GetEmailFactory().SendMail(args);
                }

            }


            return response;
        }
    }
}