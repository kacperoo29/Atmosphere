namespace Atmosphere.Core.Models;

public abstract class BaseModel
{
    protected BaseModel()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; }
    public DateTime CreatedAt { get; }
    public DateTime UpdatedAt { get; private set; }

    protected void Update()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}