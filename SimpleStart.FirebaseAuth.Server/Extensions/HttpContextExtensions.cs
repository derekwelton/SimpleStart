using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SimpleStart.Auth.Firebase.Models;
using SimpleStart.Core.Extensions;

namespace SimpleStart.Auth.Firebase.Extensions;

public static class HttpContextExtensions
{
    public static FirebaseUser GetFirebaseUser(this HttpContext httpContext)
    {
        var user = httpContext.User;

        string id = user.FindFirstValue(FirebaseUserClaimType.ID);
        string email = user.FindFirstValue(FirebaseUserClaimType.EMAIL);
        string username = user.FindFirstValue(FirebaseUserClaimType.USERNAME);
        bool.TryParse(user.FindFirstValue(FirebaseUserClaimType.EMAIL_VERIFIED), out bool emailVerified);

        var firebaseUser = new FirebaseUser(id, email, username, emailVerified);
        foreach (var claim in user.Claims.Where(x => x.Type.Contains("role_")))
        {
            if(claim.Value.ToBool()) firebaseUser.Roles.Add(claim.Type.Substring("role_".Length));
        }

        

        return firebaseUser;
    }
}