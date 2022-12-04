using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Microsoft.Extensions.Options;
using SimpleStart.Auth.Firebase.Models;

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
    public async Task<FirebaseUser> TryCreateRootAdmin(string adminEmail, string adminPassword = "secretpassword")
    {
        try
        {
            //make sure user doesn't exist already
            var authUser = await _firebaseClient.GetUserByEmailAsync(adminEmail);
            return new FirebaseUser(authUser);
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
                PhoneNumber = "+12345678900",
                PhotoUrl = "http://www.example.com/12345678/photo.png"
            };

            var authUser = await _firebaseClient.CreateUserAsync(args);

            //add admin role to user
            var claims = new Dictionary<string, object>();
            claims.Add("admin",true);
            await UpdateCustomUserClaims(authUser.Uid, claims);

            authUser = await _firebaseClient.GetUserByEmailAsync(adminEmail);
            return new FirebaseUser(authUser);
        }
    }

    /// <summary>
    /// This will replace all custom claims. Make sure everytime this is called,
    /// that you pass in all the existing custom claims you wish to keep.
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="roles"></param>
    /// <returns></returns>
    public Task UpdateCustomUserClaims(string uid, Dictionary<string,object> claims)
    {
        return _firebaseClient.SetCustomUserClaimsAsync(uid, claims);
    }
    /// <summary>
    /// Gets all the user's custom claims.
    /// </summary>
    /// <param name="uid"></param>
    /// <returns></returns>
    public async Task<Dictionary<string, object>> GetUserCustomClaims(string uid)
    {
        var authUser = await GetAuthUserByIdAsync(uid);
        return authUser!.CustomClaims;
    }


    /// <summary>
    /// Creates a new user in firebase auth,sets the id of IAuthUser parameter and sends an email.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task RegisterUserAsync(FirebaseUser user)
    {
        if (_firebaseConfig.EmailIsOn) throw new Exception("Firebase Email Provider is not configured properly");
        var password = RandomString(12);
        await RegisterUserAsync(user,password);

        var passwordResetLink = await _firebaseClient.GeneratePasswordResetLinkAsync(user.Email);
        await _emailProvider.SendRegisterPasswordResetEmail(user.Email, user.DisplayName, passwordResetLink, _firebaseConfig.RegisterSubjectLine);
    }
    /// <summary>
    /// Creates a new user in firebase auth
    /// </summary>
    /// <param name="user"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public async Task RegisterUserAsync(FirebaseUser user, string password)
    {
        var args = new UserRecordArgs()
        {
            Email = user.Email,
            EmailVerified = user.EmailVerified,
            DisplayName = user.DisplayName,
            PhotoUrl = (string.IsNullOrEmpty(user.PhotoUrl)) ? "http://www.example.com/12345678/photo.png" : user.PhotoUrl,
            Disabled = !user.Disabled,
            Password = password,
            PhoneNumber = user.PhoneNumber
        };

        var authUser = await _firebaseClient.CreateUserAsync(args);
        if (user.CustomClaims is {Count: > 0}) await UpdateCustomUserClaims(authUser.Uid, user.CustomClaims);
        user.Id = authUser.Uid;
    }
    /// <summary>
    /// Delete the user from Firebase Auth
    /// </summary>
    /// <param name="uid"></param>
    /// <returns></returns>
    public Task DeleteUserAsync(string uid)
    {
        return _firebaseClient.DeleteUserAsync(uid);
    }
    /// <summary>
    /// Get firebase user by email
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public async Task<FirebaseUser?> GetAuthUserByEmailAsync(string email)
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
    public async Task<FirebaseUser?> GetAuthUserByIdAsync(string uid)
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
    /// <param name="email"></param>
    /// <param name="displayName"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task SendPasswordResetAsync(string email, string displayName)
    {
        if (_firebaseConfig.EmailIsOn == false)
            throw new Exception("Firebase Email Provider is not configured properly");

        var link = await GetPasswordResetLinkAsync(email);
        await _emailProvider.SendPasswordResetEmail(email, displayName, link,
            _firebaseConfig.ResetPasswordSubjectLine);
    }
    /// <summary>
    /// Get a password reset link to use in your custom password reset solution
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public Task<string> GetPasswordResetLinkAsync(string email)
    {
        return _firebaseClient.GeneratePasswordResetLinkAsync(email);
    }

    /// <summary>
    /// Change the firebase user's password
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public Task UpdatePasswordAsync(string uid, string password)
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
    public Task UpdatePhotoUrlAsync(string uid, string photoUrl)
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
    public Task UpdateEmailAsync(string uid, string email)
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
    public Task UpdateDisplayNameAsync(string uid, string displayName)
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
    public Task UpdateDisabledAsync(string uid, bool disabled)
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
    public Task UpdateEmailVerifiedAsync(string uid, bool emailVerified)
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
    public Task UpdateUserAsync(FirebaseUser user)
    {
        UserRecordArgs args = new UserRecordArgs()
        {
            Uid = user.Id,
            Email = user.Email,
            EmailVerified = user.EmailVerified,
            DisplayName = user.DisplayName,
            Disabled = !user.Disabled,
            PhotoUrl = user.PhotoUrl,
        };

        return _firebaseClient.UpdateUserAsync(args);
    }
    /// <summary>
    /// validates the token
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task<bool> IsTokenValidAsync(string token)
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
    public Task RevokeAllTokensAsync(string uid)
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