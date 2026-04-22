using api.Services.DTOs;

namespace api.Services;

public interface IUserService
{
    Task<IEnumerable<UserResponse>> GetUsersAsync();
    Task<UserResponse?> GetUserByIdAsync(int id);
    Task<UserResponse> CreateUserAsync(UserCreateRequest request);
    Task<bool> DeleteUserAsync(int id);
}