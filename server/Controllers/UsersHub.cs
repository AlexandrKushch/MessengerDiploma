using Microsoft.AspNetCore.SignalR;
using Server.Models.Request;
using Server.Services.Interfaces;

namespace Server.Controllers
{
    public class UsersHub : Hub
    {
        private readonly IUserService _userService;

        public UsersHub(IUserService userService)
        {
            _userService = userService;
        }

        public async void Register(UserCreateDto dto)
        {
            try
            {
                var user = await _userService.CreateAsync(dto);
                await Clients.All.SendAsync("OnUserRegistered", user);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}