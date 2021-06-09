using System;
using System.ComponentModel.DataAnnotations;

namespace DowntimeAlerter.Domain.Entities
{
    public class BaseEntity
    {
        [Key] public Guid Id { get; set; }
    }
}