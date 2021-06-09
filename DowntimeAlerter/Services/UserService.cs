using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DowntimeAlerter.Data.DbContext;
using DowntimeAlerter.Domain.Common;
using DowntimeAlerter.Infrastructure.Helper;
using DowntimeAlerter.Infrastructure.Helper.Contract;
using DowntimeAlerter.Infrastructure.ViewModel;
using DowntimeAlerter.Infrastructure.ViewModel.Request;
using DowntimeAlerter.Infrastructure.ViewModel.Response;
using DowntimeAlerter.Services.Contract;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;

namespace DowntimeAlerter.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _manager;
        private readonly IMapper _mapper;
        private readonly IJwtTokenGenerator _jwtGenerator;
        private ApplicationDbContext _context;

        public UserService(UserManager<IdentityUser> manager, IMapper mapper, IJwtTokenGenerator jwtGenerator,
            ApplicationDbContext context)
        {
            _manager = manager;
            _mapper = mapper;
            _jwtGenerator = jwtGenerator;
            _context = context;
        }

        public ApiResponse<List<UserModel>> GetAll()
        {
            var users = _manager.Users;
            return new ApiResponse<List<UserModel>>(_mapper.Map<List<UserModel>>(users));
        }

        public ApiResponse<UserModel> Get(string id)
        {
            var user = _manager.Users.FirstOrDefault(identityUser => identityUser.Id == id);

            if (user == null) throw new CustomException("User could not found");
            return new ApiResponse<UserModel>(_mapper.Map<UserModel>(user));
        }

        public async Task<ApiResponse<LoginResultModel>> Login(LoginModel model)
        {
            var user = _manager.Users.FirstOrDefault(identityUser => identityUser.UserName == model.UserName);
            if (user == null) throw new CustomException("User could not found");
            return new ApiResponse<LoginResultModel>(new LoginResultModel
            {
                User = _mapper.Map<UserModel>(user),
                AccessToken = await _jwtGenerator.Generate(new[]
                {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                })
            });
        }

        public async Task<ApiResponse<string>> Register(UserRegisterModel model)
        {
            var isEmailExist = await _manager.FindByEmailAsync(model.Email);
            var validationErrorList = new List<string>();

            if (isEmailExist != null)
                validationErrorList.Add("Email Already Taken");

            var isUserNameExist = await _manager.FindByNameAsync(model.UserName);
            if (isUserNameExist != null)
                validationErrorList.Add("UserName Already Taken");

            var isPhoneNumberExist =
                _context.Users.FirstOrDefault(u => u.PhoneNumber.Equals(model.PhoneNumber));
            if (isPhoneNumberExist != null)
                validationErrorList.Add("Phone Number Already Taken");

            if (validationErrorList.Any())
                throw new CustomException(validationErrorList);

            var user = new IdentityUser()
            {
                Email = model.Email,
                NormalizedEmail = model.Email.Normalize(),
                PhoneNumber = model.PhoneNumber,
                UserName = model.UserName,
                NormalizedUserName = model.UserName.Normalize()
            };

            var result = await _manager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return new ApiResponse<string>(
                    user.Id,
                    "User Successfully Registered");
            }

            var errorList = new List<string>();
            for (var i = 0; i < result.Errors.Count(); i++)
                errorList.Add(result.Errors.ToList()[i].Description);

            throw new CustomException(errorList);
        }

        public async Task<ApiResponse<string>> Update(UserUpdateModel model)
        {
            var validationErrorList = new List<string>();

            var userWithNewEmail = await _manager.FindByEmailAsync(model.Email);
            if (userWithNewEmail != null && userWithNewEmail.Id != model.Id)
                validationErrorList.Add("Email Already Taken");

            var userWithNewPhoneNumber =
                _context.Users.FirstOrDefault(u => u.PhoneNumber.Equals(model.PhoneNumber));
            if (userWithNewPhoneNumber != null && userWithNewPhoneNumber.Id != model.Id)
                validationErrorList.Add("Phone Number Already Taken");

            var userWithNewUserName = await _manager.FindByNameAsync(model.UserName);
            if (userWithNewUserName != null && userWithNewUserName.Id != model.Id)
                validationErrorList.Add("UserName Already Taken");

            var user = await _manager.Users
                .FirstOrDefaultAsync(userFromDb => userFromDb.Id == model.Id);
            if (user == null)
                validationErrorList.Add("User Could Not Found");

            if (validationErrorList.Any())
                throw new CustomException(validationErrorList);

            if (user != null)
            {
                user.UserName = model.UserName;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;

                var result = await _manager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return new ApiResponse<string>("", "User Successfully Updated");
                }

                var errorArray = new List<string>();
                if (result.Errors.Any())
                    errorArray.AddRange(result.Errors.Select(res => res.Description));

                throw new CustomException(errorArray);
            }

            return new ApiResponse<string>("", "");
        }

        public async Task<ApiResponse<string>> Delete(string id)
        {
            var user = await _manager.FindByIdAsync(id);
            if (user != null)
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();
                var logins = await _manager.GetLoginsAsync(user);
                foreach (var login in logins)
                    await _manager.RemoveLoginAsync(user, login.LoginProvider, login.ProviderKey);

                var result = await _manager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    await transaction.CommitAsync();
                    return new ApiResponse<string>("", "User Successfully Deleted");
                }

                throw new CustomException("User Could Not Removed");
            }

            throw new CustomException("User Could Not Found");
        }
    }
}