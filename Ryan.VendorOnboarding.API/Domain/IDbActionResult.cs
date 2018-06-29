
using System.Collections.Generic;


namespace Ryan.VendorOnboarding.API.Domain
{
    public interface IDbActionResult
    {
        IEnumerable<string> Errors { get; set; }
        bool Succeeded { get; }
    }
}
