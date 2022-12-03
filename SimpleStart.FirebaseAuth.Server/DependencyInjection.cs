using FirebaseAdmin;
using FluentEmail.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleStart.Auth.Firebase.Handlers;
using SimpleStart.Auth.Firebase.Models;
using SimpleStart.Auth.Firebase.Services;
using SimpleStart.Core.Extensions;

namespace SimpleStart.Auth.Firebase;

public static class DependencyInjection
{
    public static IServiceCollection AddFirebaseAuthentication(this IServiceCollection services)
    {
        services.AddSingleton<FirebaseEmailProvider>(new FirebaseEmailProvider(new Email()));
        services.Configure<FirebaseAuthConfigOptions>(option => {});
        return services.AddFirebaseServices();
    }
    public static IServiceCollection AddFirebaseAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<FirebaseEmailProvider>();
        services.ConfigureEmailService(configuration);
        return services.AddFirebaseServices();
    }
    public static IServiceCollection AddFirebaseAuthentication(this IServiceCollection services, string authEmailAddress, string sendGridApiKey, string fromName, string registerSubject, string resetPasswordSubject)
    {
        services.AddSingleton<FirebaseEmailProvider>();
        services.ConfigureEmailService(authEmailAddress,sendGridApiKey,fromName, registerSubject, resetPasswordSubject);
        return services.AddFirebaseServices();
    }



    private static IServiceCollection AddFirebaseServices(this IServiceCollection services)
    {
        services.AddSingleton(FirebaseApp.Create());

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddScheme<AuthenticationSchemeOptions, FirebaseAuthenticationHandler>(JwtBearerDefaults.AuthenticationScheme, (o) => { });

        services.AddScoped<FirebaseAuthenticationFunctionHandler>();

        return services;
    }
    private static void ConfigureEmailService(this IServiceCollection services, IConfiguration configuration)
    {
        var from = configuration["FirebaseAuthConfig:FromEmail"];
        var fromName = configuration["FirebaseAuthConfig:FromName"];
        var apiKey = configuration["FirebaseAuthConfig:SendGridKey"];
        var registerSubject = configuration["FirebaseAuthConfig:RegisterSubjectLine"];
        var resetPasswordSubject = configuration["FirebaseAuthConfig:ResetPasswordSubjectLine"];

        services.Configure<FirebaseAuthConfigOptions>(option =>
        {
            option.FromEmail = fromName;
            option.FromName = fromName;
            option.SendGridKey = apiKey;
            option.RegisterSubjectLine = registerSubject;
            option.ResetPasswordSubjectLine = resetPasswordSubject;
            option.EmailIsOn = (from.IsNotNull() && fromName.IsNotNull() && apiKey.IsNotNull() && registerSubject.IsNotNull());
        });

        services.AddFluentEmail(from, fromName)
            .AddSendGridSender(apiKey);
    }
    private static void ConfigureEmailService(this IServiceCollection services, string fromEmailAddress, string sendGridApiKey, string fromName, string registerSubject, string resetPasswordSubject)
    {
        services.Configure<FirebaseAuthConfigOptions>(option =>
        {
            option.FromEmail = fromName;
            option.FromName = fromName;
            option.SendGridKey = sendGridApiKey;
            option.RegisterSubjectLine = registerSubject;
            option.ResetPasswordSubjectLine = resetPasswordSubject;
            option.EmailIsOn = (fromEmailAddress.IsNotNull() && fromName.IsNotNull() && sendGridApiKey.IsNotNull() && registerSubject.IsNotNull());
        });

        
        services.AddFluentEmail(fromEmailAddress, fromName)
            .AddSendGridSender(sendGridApiKey);
    }

}