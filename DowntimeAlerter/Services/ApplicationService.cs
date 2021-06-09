using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DowntimeAlerter.Data.UnitOfWork;
using DowntimeAlerter.Domain.Entities;
using DowntimeAlerter.Infrastructure.Helper;
using DowntimeAlerter.Infrastructure.ViewModel.Request.Application;
using DowntimeAlerter.Services.Contract;
using ApplicationModel = DowntimeAlerter.Infrastructure.ViewModel.Response.Application.ApplicationModel;

namespace DowntimeAlerter.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ApplicationService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<List<ApplicationModel>> GetAll()
        {
            return _mapper.Map<List<ApplicationModel>>(await _uow.Applications.GetAll());
        }

        public async Task<ApplicationModel> Get(string id)
        {
            return _mapper.Map<ApplicationModel>(await _uow.Applications.GetById(id));
        }

        public string Add(ApplicationAddModel model)
        {
            if (string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Url))
                throw new CustomException("Please fill all fields");
            var result = _uow.Applications.Add(_mapper.Map<Application>(model));
            _uow.Complete();
            return result.Id.ToString();
        }

        public async Task<string> Update(ApplicationUpdateModel model)
        {
            if (string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Url))
                throw new CustomException("Please fill all fields");
            var application = await _uow.Applications.GetById(model.Id);
            if (application == null) throw new CustomException("Application could not found");

            var mapped = _mapper.Map<Application>(model);
            mapped.LastUpdate = DateTime.Now;

            var result = _uow.Applications.Update(mapped);
            _uow.Complete();
            return result.Id.ToString();
        }

        public string Delete(string id)
        {
            var application = _uow.Applications.SoftDelete(id);
            return application.Id.ToString();
        }
    }
}