using System.Collections.Generic;

namespace SimpleStart.Core.Models;

public interface IAuthUser
{
    string Id { get; }
    string Email { get; }
    string Username { get; }
    bool EmailVerified { get; }
    List<string> Roles { get; set; }
}