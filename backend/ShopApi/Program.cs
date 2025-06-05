using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication("Basic")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthHandler>("Basic", null);

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "Welcome to the Simple Shop API!")
    .RequireAuthorization();

app.Run();

public class BasicAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public BasicAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
        {
            return Task.FromResult(AuthenticateResult.Fail("Missing Authorization Header"));
        }

        AuthenticationHeaderValue authHeader;
        try
        {
            authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
        }
        catch
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
        }

        var credentialBytes = Convert.FromBase64String(authHeader.Parameter ?? string.Empty);
        var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':', 2);
        if (credentials.Length != 2)
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
        }

        var username = credentials[0];
        var password = credentials[1];

        if (username != "admin" || password != "password")
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid Username or Password"));
        }

        var claims = new[] { new Claim(ClaimTypes.Name, username) };
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
