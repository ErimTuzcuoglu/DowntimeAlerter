using DowntimeAlerter.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DowntimeAlerter.Persistence
{
    public interface IApplicationDbContext
    {
        DbSet<Application> Applications { get; set; }
    }
}