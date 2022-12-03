using System.Collections.Generic;
using FirebaseAdmin.Auth;
using SimpleStart.Core.Models;

namespace SimpleStart.Auth.Firebase.Models;

public class FirebaseUser : IAuthUser
{
    public string Id { get; set; }
    public string Email { get; }
    public string Username { get; }
    public string ProfilePic { get; }
    public bool EmailVerified { get; }
    public bool Disabled { get; set; }
    public List<string> Roles { get; set; } = new List<string>();

    public FirebaseUser(string id, string email, string username, bool emailVerified)
    {
        Id = id;
        Email = email;
        Username = username;
        EmailVerified = emailVerified;
    }

    public FirebaseUser(UserRecord source)
    {
        this.Id = source.Uid;
        this.Email = source.Email;
        this.Disabled = source.Disabled;
        this.EmailVerified = source.EmailVerified;
        this.Username = source.DisplayName;
        this.ProfilePic = source.PhotoUrl;

        foreach (var customClaim in source.CustomClaims)
        {
            Roles.Add(customClaim.Key);
        }
    }

    public FirebaseUser()
    {
        
    }
}