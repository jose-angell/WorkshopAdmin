using MudBlazor;

namespace WorkshopAdmin.Client.Styles;

public static class IndustrialTheme
{
    public static MudTheme Theme => new MudTheme()
    {
        PaletteLight = new PaletteLight()
        {
            Primary = "#000666",             // El Azul Profundo de Stitch
            Secondary = "#4c616c",
            Tertiary = "#380b00",
            Background = "#f9f9f9",
            AppbarBackground = "#ffffff",    // TopBar en blanco
            Surface = "#ffffff",
            DrawerBackground = "#eeeeee",
            Error = "#ba1a1a",
            TextPrimary = "#1a1c1c",
            LinesDefault = "#767683",
            Success = "#2e7d32",
            Warning = "#ed6c02"
        },
        LayoutProperties = new LayoutProperties()
        {
            DefaultBorderRadius = "4px"
        }
    };
}