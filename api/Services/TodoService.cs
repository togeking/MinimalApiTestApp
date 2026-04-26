using api.Repositories;
using api.Repositories.Entities;
using api.Repositories.Enums;
using api.Services.DTOs;

namespace api.Services;

public class TodoService : ITodoService
{
    private readonly ITodoRepository _todoRepository;

    // 倉庫番（Repository）をDIで呼び出すぜ
    public TodoService(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }

    public async Task<IEnumerable<TodoResponse>> GetTodosAsync()
    {
        var todos = await _todoRepository.GetAllAsync();
        // LINQを使って、全件をDTOに変換して返す
        return todos.Select(MapToResponse);
    }

    public async Task<TodoResponse?> GetTodoByIdAsync(int id)
    {
        var todo = await _todoRepository.GetByIdAsync(id);
        return todo == null ? null : MapToResponse(todo);
    }

    public async Task<IEnumerable<TodoResponse>> GetTodosByUserIdAsync(int userId)
    {
        var todos = await _todoRepository.GetByUserIdAsync(userId);
        return todos.Select(MapToResponse);
    }

    public async Task<TodoResponse> CreateTodoAsync(TodoCreateRequest request)
    {
        // 1. DTOからEntityへの変換（入力された文字列をEnumに直す！）
        var todo = new TodoItem
        {
            Title = request.Title,
            UserId = request.UserId,
            AssigneeId = request.AssigneeId,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            // 文字列 "InProgress" などを Enum に変換する。失敗したら安全に NotStarted にするぜ
            Status = Enum.TryParse<TodoStatus>(request.Status, true, out var status) 
                        ? status 
                        : TodoStatus.NotStarted,
            CreatedAt = DateTime.UtcNow
        };

        // 2. 倉庫番に保存を依頼する
        await _todoRepository.AddAsync(todo);

        // 3. 採番されたIDを含めて、DTOにして返す
        return MapToResponse(todo);
    }

    public async Task<bool> UpdateTodoAsync(TodoUpdateRequest request)
    {
        // 1. まずは更新対象が存在するかチェックだ
        var existingTodo = await _todoRepository.GetByIdAsync(request.Id);
        if (existingTodo == null) return false;

        // 2. 変更箇所を上書きする
        existingTodo.Title = request.Title;
        existingTodo.UserId = request.UserId;
        existingTodo.AssigneeId = request.AssigneeId;
        existingTodo.StartDate = request.StartDate;
        existingTodo.EndDate = request.EndDate;

        if (Enum.TryParse<TodoStatus>(request.Status, true, out var status))
        {
            existingTodo.Status = status;
        }

        // 3. 倉庫番に更新を依頼する
        await _todoRepository.UpdateAsync(existingTodo);
        return true;
    }

    public async Task<bool> DeleteTodoAsync(int id)
    {
        var existingTodo = await _todoRepository.GetByIdAsync(id);
        if (existingTodo == null) return false;

        await _todoRepository.DeleteAsync(id);
        return true;
    }

    // ==========================================
    // 💡 アーキテクトの隠し味（プライベートメソッド）
    // ==========================================
    
    // Entity から DTO(TodoResponse) への変換処理を1箇所にまとめるんだ。
    // こうすれば、後から「返す項目を増やしたい」時にここだけ直せば済む！
    private static TodoResponse MapToResponse(TodoItem todo)
    {
        return new TodoResponse(
            todo.Id,
            todo.Title,
            todo.UserId,
            todo.AssigneeId,
            todo.StartDate,
            todo.EndDate,
            todo.Status.ToString(), // Enum を String に変換して返すぜ！
            todo.CreatedAt
        );
    }
}