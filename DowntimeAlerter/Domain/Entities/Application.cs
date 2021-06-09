using System;

namespace DowntimeAlerter.Domain.Entities
{
    public class Application : BaseEntity
    {
        public string Url { get; set; }
        public string Name { get; set; }

        public DateTime LastUpdate { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}