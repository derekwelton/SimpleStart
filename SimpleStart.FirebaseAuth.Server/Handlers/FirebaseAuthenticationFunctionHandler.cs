using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using SimpleStart.Auth.Firebase.Models;

namespace SimpleStart.Auth.Firebase.Handlers;

public class FirebaseAuthenticationFunctionHandler
{
    private const string BEARER_PREFIX = "Bearer ";
    private readonly FirebaseApp _firebaseApp;

    public FirebaseAuthenticationFunctionHandler(FirebaseApp firebaseApp)
    {
        _firebaseApp = firebaseApp;
    }

    public async Task<AuthenticateResult> HandleAuthenticateAsync(HttpContext context)
    {
        //Validate "Authorization" claim exist
        if(context.Request.Headers.ContainsKey("Authorization")) return AuthenticateResult.NoResult();

        //Make sure "Authorization" claim has a valid Value
        string bearerToken = context.Request.Headers["Authorization"];
        if(string.IsNullOrEmpty(bearerToken) || bearerToken.StartsWith(BEARER_PREFIX) == false) return AuthenticateResult.Fail("Invalid Schema");

        string token = bearerToken.Substring(BEARER_PREFIX.Length); //gets just the token and removes the bearer prefix

        try
        {
            var firebaseClient = FirebaseAdmin.Auth.FirebaseAuth.GetAuth(_firebaseApp); //get the client
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
            throw new Exception("Failed to verify firebase Token", ex);
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
        var claims = new List<Claim>();

        foreach (var item in source)
        {
            if(item.Key == "user_id") claims.Add(new Claim(FirebaseUserClaimType.ID,item.Value.ToString() ?? string.Empty));
            else if(item.Key == "email") claims.Add(new Claim(FirebaseUserClaimType.EMAIL,item.Value.ToString() ?? string.Empty));
            else if(item.Key == "email_verified") claims.Add(new Claim(FirebaseUserClaimType.EMAIL_VERIFIED,item.Value.ToString() ?? string.Empty));
            else if(item.Key == "name") claims.Add(new Claim(FirebaseUserClaimType.USERNAME,item.Value.ToString() ?? string.Empty));
            else claims.Add(new Claim(item.Key,item.Value.ToString() ?? string.Empty));
        }

        return claims;
    }
}