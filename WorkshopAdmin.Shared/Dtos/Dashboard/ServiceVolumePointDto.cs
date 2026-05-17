namespace WorkshopAdmin.Shared.Dtos.Dashboard;

public class ServiceVolumePointDto
{
    public string Label { get; set; } = string.Empty;

    public int Intake { get; set; }

    public int Completed { get; set; }
}