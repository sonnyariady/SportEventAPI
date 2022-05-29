using SportEventAPI.Response;
using SportEventAPI.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportEventAPI.Interface
{
    public interface IUserServices
    {
        Task<UserGlobalOutput> Create(CreateUserRequest input);
        Task<LoginResultGlobalOutput> Login(LoginRequest input);
        Task<LoginResultGlobalOutput> ChangePassword(LoginRequest input);
        Task<UserGlobalOutput> Edit(long id, BasicUserRequest input);
        Task<bool> Delete(long id);
        Task<UserResponse> GetById(long id);
    }
}
