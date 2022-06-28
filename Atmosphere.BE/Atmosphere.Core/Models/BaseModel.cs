namespace Atmosphere.Core.Models;

using System;

public abstract class BaseModel
{
    public Guid Id { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    protected BaseModel()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    protected void Update()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}