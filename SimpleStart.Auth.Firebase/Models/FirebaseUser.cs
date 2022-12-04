using System.Collections.Generic;
using FirebaseAdmin.Auth;

namespace SimpleStart.Auth.Firebase.Models;

public class FirebaseUser
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string PhotoUrl { get; set; } = string.Empty;
    public bool EmailVerified { get; set; }
    public bool Disabled { get; set; }
    public string AuthTime { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public Dictionary<string, object> CustomClaims { get; internal set; } = new Dictionary<string, object>();

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
            this.CustomClaims.Add(customClaim.Key,customClaim.Value);
        }
    }
}