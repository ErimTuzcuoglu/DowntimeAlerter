using System;

namespace DowntimeAlerter.Infrastructure.ViewModel.Response.Application
{
    public class ApplicationModel
    {
        public string Url { get; set; }
        public string Name { get; set; }

        public DateTime LastUpdate { get; set; }
    }
}