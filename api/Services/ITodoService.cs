using api.Services.DTOs;

namespace api.Services;

public interface ITodoService
{
    Task<IEnumerable<TodoResponse>> GetTodosAsync();
    Task<TodoResponse?> GetTodoByIdAsync(int id);
    Task<IEnumerable<TodoResponse>> GetTodosByUserIdAsync(int userId);
    Task<TodoResponse> CreateTodoAsync(TodoCreateRequest request);
    Task<bool> UpdateTodoAsync(TodoUpdateRequest request);
    Task<bool> DeleteTodoAsync(int id);
}