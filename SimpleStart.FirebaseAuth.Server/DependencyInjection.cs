using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using SimpleStart.Auth.Firebase.Handlers;

namespace SimpleStart.Auth.Firebase;

public static class DependencyInjection
{
    public static IServiceCollection AddFirebaseAuthentication(this IServiceCollection services)
    {
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddScheme<AuthenticationSchemeOptions, FirebaseAuthenticationHandler>(JwtBearerDefaults.AuthenticationScheme, (o) => { });

        services.AddScoped<FirebaseAuthenticationFunctionHandler>();

        return services;
    }
}