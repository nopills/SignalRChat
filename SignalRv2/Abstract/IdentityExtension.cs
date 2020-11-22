using System.Security.Claims;
using System.Security.Principal;

namespace SignalRv2.Abstract
{
    public static class IdentityExtension
    {
       public static string FullName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("FullName");
            return (claim != null) ? claim.Value : string.Empty;
        }
    }
}
