using System;

namespace DowntimeAlerter.Infrastructure.ViewModel.Request.Application
{
    public class ApplicationUpdateModel
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
    }
}