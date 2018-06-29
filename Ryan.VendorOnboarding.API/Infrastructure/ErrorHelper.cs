using System;
using System.Text;

namespace Ryan.VendorOnboarding.API.Infrastructure
{
    public class ErrorHelper
    {
        public static string[] GetAllExceptionMessages(Exception ex)
        {
            StringBuilder sbErrMsgs = new StringBuilder();

            sbErrMsgs.Append(ex.Message);
            if (ex.InnerException != null)
            {
                sbErrMsgs.Append("||");
                sbErrMsgs.Append(GetAllExceptionMessages(ex.InnerException));
            }

            string[] seperators = { "||" };
            string[] msgs = sbErrMsgs.ToString().Split(seperators, StringSplitOptions.None);
            return msgs;
        }
    }
}