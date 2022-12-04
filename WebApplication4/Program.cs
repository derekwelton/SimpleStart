using Firebase.Auth;
using Microsoft.AspNetCore.Mvc;
using SimpleStart.Auth.Firebase;
using SimpleStart.Auth.Firebase.Extensions;
using SimpleStart.Auth.Firebase.Models;
using SimpleStart.Auth.Firebase.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddFirebaseAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();


app.MapGet("/createadmin", async ([FromServices] FirebaseUserStore userStore) =>
{
    await userStore.TryCreateRootAdmin("admin@example.com", "secretPassword");

    Results.Ok();
}).AllowAnonymous();

app.MapGet("/registerUser", async ([FromServices] FirebaseUserStore userStore) =>
{
    var user = new FirebaseUser
    {
        Email = "user@example.com",
        DisplayName = "User 1",
        PhotoUrl = "http://www.example.com/12345678/photo.png",
        EmailVerified = true, 
        Disabled = false, //allows the user login to firebase auth
        PhoneNumber = "+12345678900"
    };

    await userStore.RegisterUserAsync(user, "secretpassword");

    Results.Ok();
}).AllowAnonymous();

app.MapGet("/passwordreset", async ([FromServices] FirebaseUserStore userStore, string email) =>
{
    var link = await userStore.GetPasswordResetLinkAsync(email);

    Results.Ok(link);
});

app.Run();





app.MapGet("/login", async (HttpContext context, [FromServices] FirebaseUserStore userStore) =>
{
    await userStore.TryCreateRootAdmin("admin@ironwood-mfg.com");

    var firebaseProvider = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyAijBfVyS7gG_7EX8iYcPCig2QrfeTCsMg"));

    var result = await firebaseProvider.SignInWithEmailAndPasswordAsync("admin@ironwood-mfg.com", "secretpassword");

    return Results.Ok(result.FirebaseToken);
}).WithOpenApi();

