using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using SimpleStart.Auth.Firebase.Models;

namespace SimpleStart.Auth.Firebase.Handlers;

public sealed class FirebaseAuthenticationFunctionHandler
{
    private const string AuthorizationHeader = "Authorization";
    private const string BearerPrefix = "Bearer ";
    private readonly FirebaseApp _firebaseApp;

    public FirebaseAuthenticationFunctionHandler(FirebaseApp firebaseApp)
    {
        _firebaseApp = firebaseApp;
    }

    public async Task<AuthenticateResult> HandleAuthenticateAsync(HttpContext context)
    {
        //Validate "Authorization" claim exist
        if(context.Request.Headers.ContainsKey(AuthorizationHeader) == false) return AuthenticateResult.NoResult();

        //Make sure "Authorization" claim has a valid Value
        string bearerToken = context.Request.Headers[AuthorizationHeader]!;
        if(string.IsNullOrEmpty(bearerToken) || bearerToken.StartsWith(BearerPrefix) == false) return AuthenticateResult.Fail("Invalid Schema");

        string token = bearerToken.Substring(BearerPrefix.Length); //gets just the token and removes the bearer prefix

        try
        {
            var firebaseClient = FirebaseAdmin.Auth.FirebaseAuth.GetAuth(_firebaseApp); //get the firebase client
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

    private AuthenticationTicket CreateAuthenticationTicket(FirebaseToken firebaseToken)
    {
        var identity = new ClaimsIdentity(CreateClaims(firebaseToken.Claims), JwtBearerDefaults.AuthenticationScheme);
        var user = new ClaimsPrincipal(identity);
        return new AuthenticationTicket(user, JwtBearerDefaults.AuthenticationScheme);
    }

    private List<Claim> CreateClaims(IReadOnlyDictionary<string, object> source)
    {
        return source.Select(item => new Claim(item.Key, item.Value.ToString() ?? string.Empty)).ToList();
    }
}