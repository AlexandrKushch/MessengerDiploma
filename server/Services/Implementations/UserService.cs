using Microsoft.AspNetCore.Identity;
using Server.Data.Models;
using Server.Data.Models.Repository;
using Server.Mappers;
using Server.Models.Request;
using Server.Models.Response;
using Server.Services.Interfaces;

namespace Server.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        public UserService(IUserRepository userRepository, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        public async Task<LoginGetDto> LoginAsync(UserLoginDto dto)
        {
            var user = await _userRepository.GetByEmail(dto.Email);

            if (user == null)
            {
                user = await _userRepository.GetByUserName(dto.Email);

                if (user == null)
                {
                    throw new ArgumentException("User with such email or username is not found.");
                }
            }

            bool isPasswordCorrect = await _userRepository.CheckPassword(user, dto.Password);

            if (!isPasswordCorrect)
            {
                throw new ArgumentException("Password is incorrect.");
            }

            var token = await _jwtService.GenerateTokenAsync(user);

            return new LoginGetDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Token = token
            };
        }

        public async Task<UserGetDto> CreateAsync(UserCreateDto dto)
        {
            var user = await _userRepository.GetByEmail(dto.Email);

            if (user != null)
            {
                throw new ArgumentException("User already exist");
            }

            user = UserMapper.Map(dto);
            var result = await _userRepository.Create(user, dto.Password);

            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.FirstOrDefault().Description);
            }

            var responseDto = UserMapper.Map(user);

            return responseDto;
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = await _userRepository.GetByEmail(id.ToString());
            await _userRepository.Delete(user);
        }

        public List<UserGetDto> GetAll()
        {
            var users = _userRepository.GetAll();
            var responses = users.ConvertAll<UserGetDto>(u => UserMapper.Map(u));
            return responses;
        }

        public async Task<UserGetDto> GetByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByEmail(id.ToString());
            var response = UserMapper.Map(user);
            return response;
        }

        public async Task UpdateAsync(Guid id, UserUpdateDto dto)
        {
            var user = await _userRepository.GetByEmail(id.ToString());

            user.UserName = dto.Username;

            await _userRepository.Update(user);
        }
    }
}
