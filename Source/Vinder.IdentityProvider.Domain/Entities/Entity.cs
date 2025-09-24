namespace Vinder.IdentityProvider.Domain.Entities;

public abstract class Entity
{
    public string Id { get; set; } = default!;

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public void MarkAsDeleted() => IsDeleted = true;
    public void MarkAsUpdated() => UpdatedAt = DateTime.UtcNow;
}