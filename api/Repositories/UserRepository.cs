using System.Data;
using Dapper;
using api.Repositories.Entities;

namespace api.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IDbConnection _db;

    // DI（依存性の注入）でDBコネクションを受け取るぜ
    public UserRepository(IDbConnection db)
    {
        _db = db;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        var sql = "SELECT Id, Name, Email FROM dbo.V_User";
        return await _db.QueryAsync<User>(sql);
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        var sql = "SELECT Id, Name, Email FROM dbo.V_User WHERE Id = @Id";
        return await _db.QuerySingleOrDefaultAsync<User>(sql, new { Id = id });
    }

    public async Task AddAsync(User user)
    {
        // 相棒の得意技、OUTPUT INSERTED だ！
        var sql = @"
            INSERT INTO dbo.V_User (Name, Email) 
            OUTPUT INSERTED.Id 
            VALUES (@Name, @Email)";

        // Userエンティティをそのまま渡せば、Dapperが @Name と @Email にマッピングしてくれる
        var newId = await _db.ExecuteScalarAsync<int>(sql, user);
        
        // 採番されたIDを、参照元のEntityにセットし直す（これ重要！）
        user.ID = newId;
    }

    public async Task DeleteAsync(int id)
    {
        // 以前のコードにはなかったが、インターフェースの契約に従ってDeleteも追加しといたぜ
        var sql = "DELETE FROM dbo.V_User WHERE Id = @Id";
        await _db.ExecuteAsync(sql, new { Id = id });
    }
}