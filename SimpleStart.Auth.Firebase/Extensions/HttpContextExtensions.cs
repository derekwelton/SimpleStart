using System;
using Microsoft.AspNetCore.Http;
using SimpleStart.Auth.Firebase.Models;

namespace SimpleStart.Auth.Firebase.Extensions;

public static class HttpContextExtensions
{
    public static FirebaseUser GetFirebaseUser(this HttpContext httpContext)
    {
        var user = httpContext.User;

        var firebaseUser = new FirebaseUser();
        foreach (var claim in user.Claims)
        {
            if (claim.Type == FirebaseClaimType.Id) firebaseUser.Id = claim.Value;
            else if (claim.Type == FirebaseClaimType.Email) firebaseUser.Email = claim.Value;
            else if (claim.Type == FirebaseClaimType.DisplayName) firebaseUser.DisplayName= claim.Value;
            else if (claim.Type == FirebaseClaimType.EmailVerified) firebaseUser.EmailVerified = Convert.ToBoolean(claim.Value);
            else if (claim.Type == FirebaseClaimType.AuthTime) firebaseUser.AuthTime = claim.Value;
            else if (claim.Type == FirebaseClaimType.PhoneNumber) firebaseUser.PhoneNumber = claim.Value;
            else if (claim.Type == FirebaseClaimType.PhotoUrl) firebaseUser.PhotoUrl = claim.Value;
            else if (claim.Type == FirebaseClaimType.AppUserId) firebaseUser.AppUserId = claim.Value;
            else firebaseUser.CustomClaims.Add(claim.Type,claim.Value);
        }

        return firebaseUser;
    }
}