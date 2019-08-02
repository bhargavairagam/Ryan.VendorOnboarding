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
                sbErrMsgs.Append(ex.InnerException.Message);
                sbErrMsgs.Append("||");
                sbErrMsgs.Append(ex.InnerException.Source);
                sbErrMsgs.Append("||");
                sbErrMsgs.Append(ex.StackTrace);
                sbErrMsgs.Append("||");
                sbErrMsgs.Append(ex.Message);
            }

            string[] seperators = { "||" };
            string[] msgs = sbErrMsgs.ToString().Split(seperators, StringSplitOptions.None);
            return msgs;
        }
    }
}