using api.Repositories;
using api.Repositories.Entities;
using api.Services.DTOs;

namespace api.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserResponse>> GetUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Select(u => new UserResponse(u.ID, u.Name, u.Email));
    }

    public async Task<UserResponse?> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return user == null ? null : new UserResponse(user.ID, user.Name, user.Email);
    }

    public async Task<UserResponse> CreateUserAsync(UserCreateRequest request)
    {
        var user = new User { Name = request.Name, Email = request.Email };
        await _userRepository.AddAsync(user);
        return new UserResponse(user.ID, user.Name, user.Email);
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return false;

        await _userRepository.DeleteAsync(id);
        return true;
    }
}