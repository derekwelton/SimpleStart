using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Microsoft.Extensions.Options;
using SimpleStart.Auth.Firebase.Models;
using SimpleStart.Core.Extensions;
using SimpleStart.Core.Models;

namespace SimpleStart.Auth.Firebase.Services;

public class FirebaseUserStore
{
    private readonly FirebaseApp _firebaseApp;
    private readonly FirebaseEmailProvider _emailProvider;
    private readonly FirebaseAuthConfigOptions _firebaseConfig;

    private FirebaseAuth _firebaseClient => FirebaseAuth.GetAuth(_firebaseApp);

    public FirebaseUserStore(FirebaseApp firebaseApp, FirebaseEmailProvider emailProvider, IOptions<FirebaseAuthConfigOptions> options)
    {
        _firebaseApp = firebaseApp;
        _emailProvider = emailProvider;
        _firebaseConfig = options.Value;
    }

    /// <summary>
    /// Creates the Rootadmin if it doesn't already exist
    /// </summary>
    /// <param name="adminEmail"></param>
    /// <param name="adminPassword"></param>
    /// <returns></returns>
    public async Task<IAuthUser> TryCreateRootAdmin(string adminEmail, string adminPassword = "secretpassword")
    {
        try
        {
            //make sure user doesn't exist already
            var authUser = await _firebaseClient.GetUserByEmailAsync(adminEmail);
            return null;
        }
        catch
        {
            var args = new UserRecordArgs()
            {
                Email = adminEmail,
                EmailVerified = true,
                Password = adminPassword,
                DisplayName = "Root Admin",
                Disabled = false,
            };

            var authUser = await _firebaseClient.CreateUserAsync(args);

            //add admin role to user
            var roles = new List<string>();
            roles.Add("admin");
            await SetRoles(authUser.Uid, roles);

            authUser = await _firebaseClient.GetUserByEmailAsync(adminEmail);
            return new FirebaseUser(authUser);
        }
    }

    /// <summary>
    /// replaces all the roles (replaces all the custom claims)
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="roles"></param>
    /// <returns></returns>
    public Task SetRoles(string uid, List<string> roles)
    {
        var claims = new Dictionary<string, object>();

        foreach (var role in roles)
        {
            claims.Add(role, "true");
        }

        return _firebaseClient.SetCustomUserClaimsAsync(uid, claims);
    }

    /// <summary>
    /// Creates a new user in firebase auth,sets the id of IAuthUser parameter.
    /// (optional) send a password reset email
    /// </summary>
    /// <param name="user"></param>
    /// <param name="emailSubject"></param>
    /// <param name="sendPasswordReset"></param>
    /// <returns></returns>
    public async Task RegisterUser(IAuthUser user, bool sendPasswordReset = true)
    {
        var password = RandomString(12);
        await RegisterUser(user,password);

        if (_firebaseConfig.EmailIsOn && sendPasswordReset)
        {
            var passwordResetLink = await _firebaseClient.GeneratePasswordResetLinkAsync(user.Email);
            await _emailProvider.SendRegisterPasswordResetEmail(user.Email, user.Username, passwordResetLink, _firebaseConfig.RegisterSubjectLine);
        }
    }
    /// <summary>
    /// Creates a new user in firebase auth
    /// </summary>
    /// <param name="user"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public async Task RegisterUser(IAuthUser user, string password)
    {
        var args = new UserRecordArgs()
        {
            Email = user.Email,
            EmailVerified = user.EmailVerified,
            DisplayName = user.Username,
            PhotoUrl = (string.IsNullOrEmpty(user.ProfilePic)) ? "http://www.example.com/12345678/photo.png" : user.ProfilePic,
            Disabled = !user.Disabled
        };

        args.Password = password;

        var authUser = await _firebaseClient.CreateUserAsync(args);
        if (user.Roles.IsNotNullOrEmpty()) await SetRoles(authUser.Uid, user.Roles);
        user.Id = authUser.Uid;
    }
    /// <summary>
    /// Delete the user from Firebase Auth
    /// </summary>
    /// <param name="uid"></param>
    /// <returns></returns>
    public Task DeleteUser(string uid)
    {
        return _firebaseClient.DeleteUserAsync(uid);
    }
    /// <summary>
    /// Get firebase user by email
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public async Task<IAuthUser?> GetAuthUserByEmailAsync(string email)
    {
        try
        {
            var result = await _firebaseClient.GetUserByEmailAsync(email);
            return new FirebaseUser(result);
        }
        catch
        {
            return null;
        }

    }
    /// <summary>
    /// Get firebase user by id
    /// </summary>
    /// <param name="uid"></param>
    /// <returns></returns>
    public async Task<IAuthUser?> GetAuthUserByIdAsync(string uid)
    {
        try
        {
            var result = await _firebaseClient.GetUserAsync(uid);
            return new FirebaseUser(result);
        }
        catch
        {
            return null;
        }

    }
    /// <summary>
    /// sends password reset email to the user
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task SendPasswordReset(IAuthUser user)
    {
        if (_firebaseConfig.EmailIsOn == false)
            throw new Exception("Firebase Email Provider is not configured properly");

        var link = await GetPasswordResetLink(user.Email);
        await _emailProvider.SendPasswordResetEmail(user.Email, user.Username, link,
            _firebaseConfig.ResetPasswordSubjectLine);
    }
    /// <summary>
    /// Get a password reset link to use in your custom password reset solution
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public Task<string> GetPasswordResetLink(string email)
    {
        return _firebaseClient.GeneratePasswordResetLinkAsync(email);
    }

    /// <summary>
    /// Change the firebase user's password
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public Task ChangePassword(string uid, string password)
    {
        UserRecordArgs args = new UserRecordArgs()
        {
            Uid = uid,
            Password = password
        };

        return _firebaseClient.UpdateUserAsync(args);
    }
    /// <summary>
    /// Change the firebase user's photo url
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="photoUrl"></param>
    /// <returns></returns>
    public Task ChangePhotoUrl(string uid, string photoUrl)
    {
        UserRecordArgs args = new UserRecordArgs()
        {
            Uid = uid,
            PhotoUrl = photoUrl
        };

        return _firebaseClient.UpdateUserAsync(args);
    }
    /// <summary>
    /// Change the firebase user's email
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="email"></param>
    /// <returns></returns>
    public Task ChangeEmail(string uid, string email)
    {
        UserRecordArgs args = new UserRecordArgs()
        {
            Uid = uid,
            Email = email
        };

        return _firebaseClient.UpdateUserAsync(args);
    }
    /// <summary>
    /// Change the firebase user's display name
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="displayName"></param>
    /// <returns></returns>
    public Task ChangeDisplayName(string uid, string displayName)
    {
        UserRecordArgs args = new UserRecordArgs()
        {
            Uid = uid,
            DisplayName = displayName
        };

        return _firebaseClient.UpdateUserAsync(args);
    }
    /// <summary>
    /// Change the firebase user's disabled field
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="disabled"></param>
    /// <returns></returns>
    public Task ChangeDisabled(string uid, bool disabled)
    {
        UserRecordArgs args = new UserRecordArgs()
        {
            Uid = uid,
            Disabled = disabled
        };

        return _firebaseClient.UpdateUserAsync(args);
    }
    /// <summary>
    /// change the firebase user's email verified field
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="emailVerified"></param>
    /// <returns></returns>
    public Task ChangeEmailVerified(string uid, bool emailVerified)
    {
        UserRecordArgs args = new UserRecordArgs()
        {
            Uid = uid,
            EmailVerified = emailVerified
        };

        return _firebaseClient.UpdateUserAsync(args);
    }
    /// <summary>
    /// update all the firebase user's fields
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public Task UpdateUser(IAuthUser user)
    {
        UserRecordArgs args = new UserRecordArgs()
        {
            Uid = user.Id,
            Email = user.Email,
            EmailVerified = user.EmailVerified,
            DisplayName = user.Username,
            Disabled = !user.Disabled,
            PhotoUrl = user.ProfilePic
        };

        return _firebaseClient.UpdateUserAsync(args);
    }
    /// <summary>
    /// validates the token
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task<bool> IsTokenValid(string token)
    {
        try
        {
            var result = await _firebaseClient.VerifySessionCookieAsync(token);
            return true;
        }
        catch
        {
            return false;
        }
    }
    /// <summary>
    /// revokes all the refresh tokens
    /// </summary>
    /// <param name="uid"></param>
    /// <returns></returns>
    public Task RevokeAllTokens(string uid)
    {
        return _firebaseClient.RevokeRefreshTokensAsync(uid);
    }

    private static readonly Random Random = new Random();
    private static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[Random.Next(s.Length)]).ToArray());
    }
}