using Server.Models.Request;
using Server.Models.Response;

namespace Server.Services.Interfaces
{
    public interface IUserService
    {
        Task<LoginGetDto> LoginAsync(UserLoginDto dto);

        Task<UserGetDto> GetByIdAsync(Guid id);

        List<UserGetDto> GetAll();

        Task<UserGetDto> CreateAsync(UserCreateDto dto);

        Task UpdateAsync(Guid id, UserUpdateDto dto);
        
        Task DeleteAsync(Guid id);
    }
}
