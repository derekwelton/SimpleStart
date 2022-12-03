using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SimpleStart.Auth.Firebase.Models;
using SimpleStart.Core.Extensions;
using SimpleStart.Core.Models;

namespace SimpleStart.Auth.Firebase.Extensions;

public static class HttpContextExtensions
{
    private const string RolePrefix = "role_";

    public static IAuthUser GetAuthUser(this HttpContext httpContext)
    {
        var user = httpContext.User;

        string id = user.FindFirstValue(FirebaseUserClaimType.Id);
        string email = user.FindFirstValue(FirebaseUserClaimType.Email);
        string username = user.FindFirstValue(FirebaseUserClaimType.Username);
        bool.TryParse(user.FindFirstValue(FirebaseUserClaimType.EmailVerified), out bool emailVerified);

        var firebaseUser = new FirebaseUser(id, email, username, emailVerified);
        foreach (var claim in user.Claims.Where(x => x.Type.Contains(RolePrefix)))
        {
            if(claim.Value.ToBool()) firebaseUser.Roles.Add(claim.Type.Substring(RolePrefix.Length));
        }

        return firebaseUser;
    }
}