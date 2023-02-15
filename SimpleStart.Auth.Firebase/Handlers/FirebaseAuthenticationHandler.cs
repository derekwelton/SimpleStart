using System.Collections.Generic;
using System.Security.Claims;
using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using FirebaseAdmin.Auth;
using FirebaseAdmin;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SimpleStart.Auth.Firebase.Models;

namespace SimpleStart.Auth.Firebase.Handlers;

public class FirebaseAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly FirebaseApp _firebaseApp;
    private const string AuthorizationHeader = "Authorization";
    private const string BearerPrefix = "Bearer ";

    public FirebaseAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock,
        FirebaseApp firebaseApp) : base(options, logger, encoder, clock)
    {
        _firebaseApp = firebaseApp;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        //Validate "Authorization" claim exist
        if (Context.Request.Headers.ContainsKey(AuthorizationHeader) == false) return AuthenticateResult.NoResult();

        //Get Token
        var token = GetToken();
        if (string.IsNullOrEmpty(token)) return AuthenticateResult.Fail("Invalid Schema");

        try
        {
            var firebaseClient = FirebaseAuth.GetAuth(_firebaseApp); //get the firebase client
            var firebaseToken = await firebaseClient.VerifyIdTokenAsync(token); //verify Token

            return AuthenticateResult.Success(CreateAuthenticationTicket(firebaseToken));
        }
        catch (FirebaseAuthException authException)
        {
            return AuthenticateResult.Fail(authException);
        }
        catch (Exception ex)
        {
            //if this happens, its because the FirebaseAdmin api is not working or the network is down.
            throw new Exception("Failed to verify firebase Token. Failed to access the Firebase Api or the network is down.", ex);
        }
    }

    private string GetToken()
    {
        string bearerToken = Context.Request.Headers[AuthorizationHeader]!;
        return IsValidBearerToken(bearerToken) == false
            ? string.Empty
            : bearerToken.Substring(BearerPrefix.Length);
    }
    private bool IsValidBearerToken(string token)
    {
        return string.IsNullOrEmpty(token) == false && token.StartsWith(BearerPrefix);
    }
    private AuthenticationTicket CreateAuthenticationTicket(FirebaseToken firebaseToken)
    {
        var identity = new ClaimsIdentity(CreateClaims(firebaseToken.Claims), JwtBearerDefaults.AuthenticationScheme);
        var user = new ClaimsPrincipal(identity);
        return new AuthenticationTicket(user, JwtBearerDefaults.AuthenticationScheme);
    }

    private List<Claim> CreateClaims(IReadOnlyDictionary<string, object> source)
    {
        var claims = new List<Claim>();
        foreach (var firebaseClaim in source)
        {
            if (firebaseClaim.Key == FirebaseClaimType.Role)
            {
                if (firebaseClaim.Value is Newtonsoft.Json.Linq.JArray stringRoles)
                    foreach (var role in stringRoles)
                    {
                        claims.Add(new Claim(FirebaseClaimType.Role, role!.ToString() ?? string.Empty));
                    }
            }
            else
            {
                claims.Add(new Claim(firebaseClaim.Key, firebaseClaim.Value.ToString() ?? string.Empty));
            }
        }

        return claims;
    }
}