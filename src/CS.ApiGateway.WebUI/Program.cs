using CS.ApiGateway.WebUI.ApiGateway.Orders;
using CS.ApiGateway.WebUI.ApiGateway.Products;
using CS.ApiGateway.WebUI.ApiGateway.Users;
using CS.ApiGateway.WebUI.Extensions;
using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
});

builder
    .Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddKeycloakWebApp(
        builder.Configuration.GetSection(KeycloakAuthenticationOptions.Section),
        configureOpenIdConnectOptions: options =>
        {
            // we need this for front-channel sign-out
            options.SaveTokens = true;
            options.ResponseType = OpenIdConnectResponseType.Code;
            options.Scope.Add("cs-apigateway-usersapi");
            options.Events = new OpenIdConnectEvents
            {
                OnSignedOutCallbackRedirect = context =>
                {
                    context.Response.Redirect("/Home/Public");
                    context.HandleResponse();

                    return Task.CompletedTask;
                }
            };
        }
    );

builder
    .Services.AddKeycloakAuthorization(builder.Configuration)
    .AddAuthorizationBuilder()
    .AddPolicy("AdminUserType", policy => policy.RequireClaim("usertype", "admin-user"));


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<AuthorizationHandler>();

builder.Services.AddHttpClient<IApiGatewayProductService, ApiGatewayProductService>(httpClient =>
{
    var apiGatewayRefUrl = builder.Configuration["ApiGatewayRef:BaseUrl"];
    if (!string.IsNullOrWhiteSpace(apiGatewayRefUrl))
        httpClient.BaseAddress = new Uri(apiGatewayRefUrl);
}).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true,
    CheckCertificateRevocationList = false
}).AddHttpMessageHandler<AuthorizationHandler>();

builder.Services.AddHttpClient<IApiGatewayOrderService, ApiGatewayOrderService>(httpClient =>
{
    var apiGatewayRefUrl = builder.Configuration["ApiGatewayRef:BaseUrl"];
    if (!string.IsNullOrWhiteSpace(apiGatewayRefUrl))
        httpClient.BaseAddress = new Uri(apiGatewayRefUrl);
}).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true,
    CheckCertificateRevocationList = false
}).AddHttpMessageHandler<AuthorizationHandler>();

builder.Services.AddHttpClient<IApiGatewayUserService, ApiGatewayUserService>(httpClient =>
{
    var apiGatewayRefUrl = builder.Configuration["ApiGatewayRef:BaseUrl"];
    if (!string.IsNullOrWhiteSpace(apiGatewayRefUrl))
        httpClient.BaseAddress = new Uri(apiGatewayRefUrl);
}).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true,
    CheckCertificateRevocationList = false
}).AddHttpMessageHandler<AuthorizationHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
