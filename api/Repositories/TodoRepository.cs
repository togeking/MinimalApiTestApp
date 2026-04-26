using System.Data;
using Dapper;
using api.Repositories.Entities;

namespace api.Repositories;

public class TodoRepository : ITodoRepository
{
    private readonly IDbConnection _db;

    // DI（依存性の注入）でDBコネクションを受け取るぜ
    public TodoRepository(IDbConnection db)
    {
        _db = db;
    }

    public async Task<IEnumerable<TodoItem>> GetAllAsync()
    {
        var sql = "SELECT Id, Title, UserId, AssigneeId, StartDate, EndDate, Status FROM dbo.V_Todo";
        return await _db.QueryAsync<TodoItem>(sql);
    }

    public async Task<TodoItem?> GetByIdAsync(int id)
    {
        var sql = "SELECT Id, Title, UserId, AssigneeId, StartDate, EndDate, Status FROM dbo.V_Todo WHERE Id = @Id";
        return await _db.QuerySingleOrDefaultAsync<TodoItem>(sql, new { Id = id });
    }

    public async Task<IEnumerable<TodoItem>> GetByUserIdAsync(int userId)
    {
        var sql = "SELECT Id, Title, UserId, AssigneeId, StartDate, EndDate, Status FROM dbo.V_Todo WHERE UserId = @UserId";
        return await _db.QueryAsync<TodoItem>(sql, new { UserId = userId });
    }

    public async Task AddAsync(TodoItem todo)
    {
        var sql = @"
            INSERT INTO dbo.V_Todo (Title, UserId, AssigneeId, StartDate, EndDate, Status, CreatedAt) 
            OUTPUT INSERTED.Id 
            VALUES (@Title, @UserId, @AssigneeId, @StartDate, @EndDate, @Status, @CreatedAt)";

        // TodoItemエンティティをそのまま渡せば、Dapperが各プロパティにマッピングしてくれる
        var newId = await _db.ExecuteScalarAsync<int>(sql, todo);
        
        // 採番されたIDを、参照元のEntityにセットし直す（これ重要！）
        todo.Id = newId;
    }

    public async Task UpdateAsync(TodoItem todo)
    {
        var sql = @"
            UPDATE dbo.V_Todo 
            SET Title = @Title, 
                UserId = @UserId, 
                AssigneeId = @AssigneeId, 
                StartDate = @StartDate, 
                EndDate = @EndDate, 
                Status = @Status 
            WHERE Id = @Id";

        await _db.ExecuteAsync(sql, todo);
    }

    public async Task DeleteAsync(int id)
    {
        var sql = "DELETE FROM dbo.V_Todo WHERE Id = @Id";
        await _db.ExecuteAsync(sql, new { Id = id });
    }
}