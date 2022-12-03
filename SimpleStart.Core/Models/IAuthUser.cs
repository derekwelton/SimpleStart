using System.Collections.Generic;

namespace SimpleStart.Core.Models;

public interface IAuthUser
{
    string Id { get; set; }
    string Email { get; }
    string Username { get; }
    string ProfilePic { get; }
    bool EmailVerified { get; }
    List<string> Roles { get; set; }
    bool Disabled { get; set; }
}