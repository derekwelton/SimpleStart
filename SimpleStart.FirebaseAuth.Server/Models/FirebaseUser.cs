using System.Collections.Generic;
using SimpleStart.Core.Models;

namespace SimpleStart.Auth.Firebase.Models;

public class FirebaseUser : IAuthUser
{
    public string Id { get; }
    public string Email { get; }
    public string Username { get; }
    public bool EmailVerified { get; }
    public List<string> Roles { get; set; } = new List<string>();

    public FirebaseUser(string id, string email, string username, bool emailVerified)
    {
        Id = id;
        Email = email;
        Username = username;
        EmailVerified = emailVerified;
    }

    public FirebaseUser()
    {
        
    }
}