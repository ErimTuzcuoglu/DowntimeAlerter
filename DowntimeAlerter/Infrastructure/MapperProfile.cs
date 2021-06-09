using AutoMapper;
using DowntimeAlerter.Domain.Entities;
using DowntimeAlerter.Infrastructure.ViewModel.Request.Application;
using DowntimeAlerter.Infrastructure.ViewModel.Response;
using DowntimeAlerter.Infrastructure.ViewModel.Response.Application;
using Microsoft.AspNetCore.Identity;

namespace DowntimeAlerter.Infrastructure
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<IdentityUser, UserModel>();

            CreateMap<Application, ApplicationModel>();
            CreateMap<ApplicationAddModel, Application>();
            CreateMap<ApplicationUpdateModel, Application>();
        }
    }
}