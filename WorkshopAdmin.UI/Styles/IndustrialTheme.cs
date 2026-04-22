using MudBlazor;

namespace WorkshopAdmin.Client.Styles;

public static class IndustrialTheme
{
    public static MudTheme Theme => new MudTheme()
    {
        PaletteLight = new PaletteLight()
        {
            Primary = "#000666",             // Deep Blue
            Secondary = "#4c616c",           // Secondary Gray
            Tertiary = "#380b00",            // Deep Brown
            Background = "#f9f9f9",          // Surface background
            AppbarBackground = "#000666",    // Marca corporativa en el header
            Surface = "#ffffff",             // Surface-container-lowest
            DrawerBackground = "#eeeeee",    // Surface-container
            Error = "#ba1a1a",               // Error state
            TextPrimary = "#1a1c1c",         // On-surface
            LinesDefault = "#767683",        // Outline
            Success = "#2e7d32",             // Color funcional para 'Completed'
            Warning = "#ed6c02"              // Color funcional para 'Repairing'
        },
        LayoutProperties = new LayoutProperties()
        {
            DefaultBorderRadius = "4px"      // Soft (0.25rem)
        },
    };
}