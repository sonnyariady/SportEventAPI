using SportEventAPI.Models;
using SportEventAPI.Response;
using SportEventAPI.Helper;
using SportEventAPI.Interface;
using SportEventAPI.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace SportEventAPI.Services
{
    public class UserServices : IUserServices
    {
        private readonly SportEventsContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserServices> _logger;

        public UserServices(SportEventsContext context, IConfiguration configuration, ILogger<UserServices> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<UserGlobalOutput> Create(CreateUserRequest input)
        {
            UserGlobalOutput globaloutput = new UserGlobalOutput();
            globaloutput.errors = new UserValidationItem();
            bool isValid = true;
            try
            {
                if (string.IsNullOrWhiteSpace(input.FirstName))
                {
                    globaloutput.status_code = 422;
                    globaloutput.errors.FirstName = new List<string>();
                    globaloutput.errors.FirstName.Add("The first name field is required.");
                    isValid = false;
                }
                if (string.IsNullOrWhiteSpace(input.LastName))
                {
                    globaloutput.status_code = 422;
                    globaloutput.errors.LastName = new List<string>();
                    globaloutput.errors.LastName.Add("The last name field is required.");
                    isValid = false;
                }
                if (string.IsNullOrWhiteSpace(input.Email))
                {
                    globaloutput.status_code = 422;
                    globaloutput.errors.Email = new List<string>();
                    globaloutput.errors.Email.Add("The Email field is required.");
                    isValid = false;
                }
                else
                {
                    if (!StringHelper.IsValidEmailAddress(input.Email))
                    {
                        globaloutput.status_code = 422;
                        isValid = false;
                        if (globaloutput.errors.Email == null)
                        {
                            globaloutput.errors.Email = new List<string>();
                        }
                        globaloutput.errors.Email.Add("The email must be a valid email address.");
                    }
                }
                if (string.IsNullOrWhiteSpace(input.Password))
                {
                    globaloutput.status_code = 422;
                    globaloutput.errors.Password = new List<string>();
                    globaloutput.errors.Password.Add("The Password field is required.");
                    isValid = false;
                }
                else
                {
                    if (input.Password.Length <8)
                    {
                        isValid = false;
                        globaloutput.status_code = 422;
                        if (globaloutput.errors.Password == null)
                        {
                            globaloutput.errors.Password = new List<string>();
                        }
                        globaloutput.errors.Password.Add("The Minimum Password is 8 Character Length.");
                    }
                }

                if (string.IsNullOrWhiteSpace(input.RepeatPassword))
                {
                    globaloutput.status_code = 422;
                    globaloutput.errors.RepeatPassword = new List<string>();
                    globaloutput.errors.RepeatPassword.Add("The RepeatPassword field is required.");
                    isValid = false;
                }
                else
                {
                    if (input.RepeatPassword != input.Password)
                    {
                        isValid = false;
                        globaloutput.status_code = 422;
                        if (globaloutput.errors.RepeatPassword == null)
                        {
                            globaloutput.errors.RepeatPassword = new List<string>();
                        }
                        globaloutput.errors.RepeatPassword.Add("The RepeatPassword must be same with Password.");
                    }
                }

                if (isValid)
                {
                    HashSalt hashSalt = HashSalt.GenerateSaltedHash(64, input.Password);
                    var Pwd = hashSalt.Hash;
                    var Salt = hashSalt.Salt;

                    User user = new User();
                    user.FirstName = input.FirstName;
                    user.LastName = input.LastName;
                    user.Email = input.Email;
                    user.Password = Pwd;
                    user.PasswordSalt = Salt;
                    await _context.Users.AddAsync(user);
                    await _context.SaveChangesAsync();

                    UserResponse userResponse = new UserResponse(user);
                    globaloutput.data = userResponse;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UserServices.Create");
            }
            return globaloutput;
        }

        public async Task<LoginResultGlobalOutput> Login(LoginRequest input)
        {
            LoginResultGlobalOutput globalres = new LoginResultGlobalOutput();
            bool IsValid = true;
            try
            {
                User user = await _context.Users.FirstOrDefaultAsync(a => a.Email == input.Email);

                if (user == null)
                {
                    globalres.status_code = 422;
                    IsValid = false;
                    globalres.message = "Email is not found";
                }

                HashSalt hashSalt = HashSalt.GenerateSaltedHash(64, input.Password);
                var Pwd = hashSalt.Hash;
                var Salt = hashSalt.Salt;

                var isVerifyPassword = HashSalt.VerifyPassword(input.Password, user.Password, user.PasswordSalt);

                if (!isVerifyPassword)
                {
                    globalres.status_code = 422;
                    IsValid = false;
                    globalres.message = "Invalid password";
                }

                if (IsValid)
                {

                    var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("Id", user.Id.ToString()),
                    new Claim("FirstName", user.FirstName),
                    new Claim("LastName", user.LastName),
                    new Claim("Email", user.Email)
                   };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddDays(1),
                        signingCredentials: signIn);

                    LoginResult loginResult = new LoginResult();
                    loginResult.Email = user.Email;
                    loginResult.Id = user.Id;
                    loginResult.token = new JwtSecurityTokenHandler().WriteToken(token);
                    globalres.data = loginResult;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UserServices.Login");
            }
            return globalres;
        }


        public async Task<LoginResultGlobalOutput> ChangePassword(long id, ChangePasswordRequest input)
        {
            LoginResultGlobalOutput globalres = new LoginResultGlobalOutput();
            bool IsValid = true;
            try
            {
                User user = await _context.Users.FindAsync(id); 

                if (user == null)
                {
                    globalres.status_code = 422;
                    globalres.message = "User id is not registered";
                    IsValid = false;
                }

                var isVerifyPassword = HashSalt.VerifyPassword(input.OldPassword, user.Password, user.PasswordSalt);

               

                if (!isVerifyPassword)
                {
                    globalres.status_code = 422;
                    globalres.message = "Old password is wrong";
                    IsValid = false;
                }

                if (string.IsNullOrEmpty(input.NewPassword))
                {
                    globalres.status_code = 422;
                    globalres.message = "Please input New password is wrong";
                    IsValid = false;
                }

                if (string.IsNullOrEmpty(input.RepeatPassword))
                {
                    globalres.status_code = 422;
                    globalres.message = "Please input Repeat password is wrong";
                    IsValid = false;
                }

                if (input.RepeatPassword != input.NewPassword)
                {
                    globalres.status_code = 422;
                    globalres.message = "Repeat Password is not same with New Password";
                    IsValid = false;
                }

                if (IsValid)
                {
                    HashSalt hashSalt = HashSalt.GenerateSaltedHash(64, input.NewPassword);
                    var Pwd = hashSalt.Hash;
                    var Salt = hashSalt.Salt;

                    user.Password = hashSalt.Hash;
                    user.PasswordSalt = hashSalt.Salt;
                    _context.Entry(user).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    globalres.message = "Success";

                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UserServices.ChangePassword");
            }
            return globalres;
        }

        public async Task<UserGlobalOutput> Edit(long id, BasicUserRequest input)
        {
            UserGlobalOutput globaloutput = new UserGlobalOutput();
            globaloutput.errors = new UserValidationItem();
            bool isValid = true;
            try
            {
                var usrupdate = await _context.Users.FindAsync(id);

                if (usrupdate == null)
                {
                    globaloutput.status_code = 422;
                    globaloutput.errors.Id = new List<string>();
                    globaloutput.errors.Id.Add("The User Id is not registered.");
                    isValid = false;
                }

                if (string.IsNullOrWhiteSpace(input.FirstName))
                {
                    globaloutput.status_code = 422;
                    globaloutput.errors.FirstName = new List<string>();
                    globaloutput.errors.FirstName.Add("The first name field is required.");
                    isValid = false;
                }
                if (string.IsNullOrWhiteSpace(input.LastName))
                {
                    globaloutput.status_code = 422;
                    globaloutput.errors.LastName = new List<string>();
                    globaloutput.errors.LastName.Add("The last name field is required.");
                    isValid = false;
                }
                if (string.IsNullOrWhiteSpace(input.Email))
                {
                    globaloutput.status_code = 422;
                    globaloutput.errors.Email = new List<string>();
                    globaloutput.errors.Email.Add("The Email field is required.");
                    isValid = false;
                }

                if (isValid)
                {
                    usrupdate.FirstName = input.FirstName;
                    usrupdate.LastName = input.LastName;
                    usrupdate.Email = input.Email;
                    _context.Entry(usrupdate).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    UserResponse userResponse = new UserResponse(usrupdate);
                    globaloutput.data = userResponse;
                }


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UserServices.Edit");
            }
            return globaloutput;
        }

        public async Task<bool> Delete(long id)
        {
            try
            {
                var oUser = await _context.Users.FindAsync(id);
                if (oUser == null)
                {
                    throw new Exception("Event is not found.");
                }
                _context.Users.Remove(oUser);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UserServices.Delete");
                return false;
            }

            return true;
        }

        public async Task<UserResponse> GetById(long id)
        {
            UserResponse userResponse = null;

            var oUser = await _context.Users.FindAsync(id);
            if (oUser != null)
            {
                userResponse = new UserResponse(oUser);
            }

            return userResponse;
        }

    }
}
