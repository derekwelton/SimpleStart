using System;
using System.Collections.Generic;
using System.Security.Claims;
using FirebaseAdmin.Auth;

namespace SimpleStart.Auth.Firebase.Models;

public class FirebaseUser
{
    public string Id { get; set; } = string.Empty;
    public string AppUserId { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string PhotoUrl { get; set; } = string.Empty;
    public bool EmailVerified { get; set; }
    public bool Disabled { get; set; }
    public string AuthTime { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public Dictionary<string, object> CustomClaims { get; set; } = new Dictionary<string, object>();
    public List<string> Roles { get; set; } = new List<string>();

    public FirebaseUser()
    {

    }

    internal FirebaseUser(UserRecord source)
    {
        this.Id = source.Uid;
        this.Email = source.Email;
        this.Disabled = source.Disabled;
        this.EmailVerified = source.EmailVerified;
        this.DisplayName = source.DisplayName;
        this.PhotoUrl = source.PhotoUrl;
        this.PhoneNumber = source.PhoneNumber;

        foreach (var customClaim in source.CustomClaims)
        {
            if (customClaim.Key == FirebaseClaimType.Role)
            {
                if (customClaim.Value is Newtonsoft.Json.Linq.JArray stringRoles)
                    foreach (var role in stringRoles)
                    {
                        Roles.Add(role.ToString());
                    }
                CustomClaims.Add(FirebaseClaimType.Role, customClaim.Value.ToString() ?? string.Empty);
            }
            else
            {
                CustomClaims.Add(customClaim.Key, customClaim.Value);
            }
        }
    }
}