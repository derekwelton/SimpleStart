namespace SimpleStart.Auth.Firebase.Models;

public class FirebaseAuthConfigOptions
{
    public bool EmailIsOn { get; set; }
    public string FromEmail { get; set; } = string.Empty;
    public string FromName { get; set; } = string.Empty;
    public string SendGridKey { get; set; } = string.Empty;
    public string RegisterSubjectLine { get; set; } = string.Empty;
    public string ResetPasswordSubjectLine { get; set; } = string.Empty;
}