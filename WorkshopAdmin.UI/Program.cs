using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using WorkshopAdmin.UI;
using WorkshopAdmin.UI.Auth;
using WorkshopAdmin.UI.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddAuthorizationCore();
//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddTransient<UnauthorizedHandler>();

builder.Services.AddHttpClient("WorkshopApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:7017/");
})
.AddHttpMessageHandler<UnauthorizedHandler>();

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("WorkshopApi"));

builder.Services.AddScoped<AuthenticationProviderJWT>();
builder.Services.AddScoped<AuthenticationStateProvider, AuthenticationProviderJWT>(x => x.GetRequiredService<AuthenticationProviderJWT>());
builder.Services.AddScoped<ILoginService, AuthenticationProviderJWT>(x => x.GetRequiredService<AuthenticationProviderJWT>());

builder.Services.AddScoped<AuthService>();

builder.Services.AddMudServices();
builder.Services.AddScoped<ServiceOrderClient>();
builder.Services.AddScoped<CustomerClient>();
builder.Services.AddScoped<InventoryClient>();
builder.Services.AddScoped<EquipmentClient>();
builder.Services.AddScoped<DashboardClient>();
builder.Services.AddScoped<AuthClient>();
builder.Services.AddScoped<UserClient>();


await builder.Build().RunAsync();
