namespace WorkshopAdmin.UI.Services;

public interface ILoginService
{
    Task Login(string token);

    Task Logout();
}