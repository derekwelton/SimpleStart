using FirebaseAdmin;
using FluentEmail.Core;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleStart.Auth.Firebase.Handlers;
using SimpleStart.Auth.Firebase.Models;
using SimpleStart.Auth.Firebase.Services;

namespace SimpleStart.Auth.Firebase;

public static class DependencyInjection
{
    /// <summary>
    /// Adds firebase authentication and registers all needed services. You will not be able to send emails.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddFirebaseAuthentication(this IServiceCollection services)
    {
        services.AddSingleton<FirebaseEmailProvider>(new FirebaseEmailProvider(new Email()));
        services.Configure<FirebaseAuthConfigOptions>(option => {});
        return services.AddFirebaseServices();
    }
    /// <summary>
    /// Adds firebase authentication and registers all needed services. It grabs the email configuration from appsettings.json
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddFirebaseAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<FirebaseEmailProvider>();
        services.ConfigureEmailService(configuration);
        return services.AddFirebaseServices();
    }
    /// <summary>
    /// Adds firebase authentication and registers all needed services. Passes email configuration as parameters.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="authEmailAddress"></param>
    /// <param name="sendGridApiKey"></param>
    /// <param name="fromName"></param>
    /// <param name="registerSubject"></param>
    /// <param name="resetPasswordSubject"></param>
    /// <returns></returns>
    public static IServiceCollection AddFirebaseAuthentication(this IServiceCollection services, string authEmailAddress, string sendGridApiKey, string fromName, string registerSubject, string resetPasswordSubject)
    {
        services.AddSingleton<FirebaseEmailProvider>();
        services.ConfigureEmailService(authEmailAddress,sendGridApiKey,fromName, registerSubject, resetPasswordSubject);
        return services.AddFirebaseServices();
    }



    private static IServiceCollection AddFirebaseServices(this IServiceCollection services)
    {
        services.AddSingleton<FirebaseUserManager>();
        AppOptions options = new AppOptions();
        options.Credential = GoogleCredential.FromFile("./appsettings.json");
        services.AddSingleton(FirebaseApp.Create(options));

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddScheme<AuthenticationSchemeOptions, FirebaseAuthenticationHandler>(JwtBearerDefaults.AuthenticationScheme, (o) => { });

        return services;
    }
    private static void ConfigureEmailService(this IServiceCollection services, IConfiguration configuration)
    {
        var fromEmail = configuration["FirebaseAuthConfig:FromEmail"];
        var fromName = configuration["FirebaseAuthConfig:FromName"];
        var apiKey = configuration["FirebaseAuthConfig:SendGridKey"];
        var registerSubject = configuration["FirebaseAuthConfig:RegisterSubjectLine"];
        var resetPasswordSubject = configuration["FirebaseAuthConfig:ResetPasswordSubjectLine"];

        services.Configure<FirebaseAuthConfigOptions>(option =>
        {
            option.FromEmail = fromEmail!;
            option.FromName = fromName!;
            option.SendGridKey = apiKey!;
            option.RegisterSubjectLine = registerSubject!;
            option.ResetPasswordSubjectLine = resetPasswordSubject!;
            option.EmailIsOn = (string.IsNullOrEmpty(fromEmail) == false && string.IsNullOrEmpty(fromName) == false
                && string.IsNullOrEmpty(apiKey) == false && string.IsNullOrEmpty(registerSubject) == false);
        });

        services.AddFluentEmail(fromEmail, fromName)
            .AddSendGridSender(apiKey);
    }
    private static void ConfigureEmailService(this IServiceCollection services, string fromEmailAddress, string sendGridApiKey, string fromName, string registerSubject, string resetPasswordSubject)
    {
        services.Configure<FirebaseAuthConfigOptions>(option =>
        {
            option.FromEmail = fromEmailAddress;
            option.FromName = fromName;
            option.SendGridKey = sendGridApiKey;
            option.RegisterSubjectLine = registerSubject;
            option.ResetPasswordSubjectLine = resetPasswordSubject;
            option.EmailIsOn = (string.IsNullOrEmpty(fromEmailAddress) == false && string.IsNullOrEmpty(fromName) == false 
                && string.IsNullOrEmpty(sendGridApiKey) == false && string.IsNullOrEmpty(registerSubject) == false);
        });

        
        services.AddFluentEmail(fromEmailAddress, fromName)
            .AddSendGridSender(sendGridApiKey);
    }

}