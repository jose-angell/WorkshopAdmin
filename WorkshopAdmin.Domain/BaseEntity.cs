namespace WorkshopAdmin.Domain;

public abstract class BaseEntity
{
    public DateTimeOffset CreatedAt { get; set; }

    public Guid CreatedByUserId { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }

    public Guid? UpdatedByUserId { get; set; }
}
