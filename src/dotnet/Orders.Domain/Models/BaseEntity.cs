﻿using System.ComponentModel.DataAnnotations;

namespace Orders.Domain.Models;

public abstract class BaseEntity
{
    [Key]
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}