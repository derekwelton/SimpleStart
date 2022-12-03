using FluentEmail.Core;
using System.Threading.Tasks;
using System;

namespace SimpleStart.Auth.Firebase.Services;

public class FirebaseEmailProvider
{
    private readonly IFluentEmail _fluentEmail;

    public FirebaseEmailProvider(IFluentEmail fluentEmail)
    {
        _fluentEmail = fluentEmail;
    }

    /// <summary>
    /// Sends an email asking the user to change their password on registration. Uses the default body.
    /// </summary>
    /// <param name="to"></param>
    /// <param name="displayName"></param>
    /// <param name="link"></param>
    /// <param name="subject"></param>
    /// <returns></returns>
    public Task SendRegisterPasswordResetEmail(string to, string displayName, string link, string subject = "App Registration")
    {
        var htmlBody = $"<html> <body> <h2>Hey {displayName},</h2> <p>You need to set your password in order to use DRP. Use the link below.</p> </br> <p><a href=\"{link}\">Set Password</a> </p> </body> </html>";

        return SendRegisterPasswordResetEmail(to, htmlBody, subject);
    }
    /// <summary>
    /// Sends an email asking the user to change their password on registration. You need to provide the body of the email
    /// </summary>
    /// <param name="to"></param>
    /// <param name="displayName"></param>
    /// <param name="link"></param>
    /// <param name="htmlBody"></param>
    /// <param name="subject"></param>
    /// <returns></returns>
    public Task SendRegisterPasswordResetEmail(string to,  string htmlBody, string subject)
    {
        var email = _fluentEmail
            .To(to)
            .Subject(subject)
            .Body(htmlBody, true)
            .Tag(Guid.NewGuid().ToString());

        return email.SendAsync();
    }

    
    /// <summary>
    /// Sends an email with a password reset link. Uses the default body.
    /// </summary>
    /// <param name="to"></param>
    /// <param name="displayName"></param>
    /// <param name="link"></param>
    /// <param name="subject"></param>
    /// <returns></returns>
    public Task SendPasswordResetEmail(string to, string displayName, string link, string subject = "Password Reset")
    {
        var htmlBody = $"<html> <body> <h2>Hey {displayName},</h2> <p>Use the link below to reset your password.</p> </br> <p><a href=\"{link}\">Reset Password</a> </p> </body> </html>";

        return SendPasswordResetEmail(to, htmlBody, subject);
    }
    /// <summary>
    /// Sends an email with a password reset link. You need to provide the body with the link
    /// </summary>
    /// <param name="to"></param>
    /// <param name="displayName"></param>
    /// <param name="link"></param>
    /// <param name="htmlBody"></param>
    /// <param name="subject"></param>
    /// <returns></returns>
    public Task SendPasswordResetEmail(string to, string htmlBody, string subject)
    {
        var email = _fluentEmail
            .To(to)
            .Subject(subject)
            .Body(htmlBody, true)
            .Tag(Guid.NewGuid().ToString());

        return email.SendAsync();
    }
}