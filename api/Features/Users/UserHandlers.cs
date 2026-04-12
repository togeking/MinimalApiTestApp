using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Data;
using Dapper;

namespace api.Features.Users;

public static class UserHandlers
{
    // 一覧取得
    // ※Dapperの戻り値は IEnumerable になるので、Listから変更しておくと自然だ
    public static async Task<Results<Ok<IEnumerable<User>>, NotFound>> GetAllUsers(IDbConnection db)
    {
        var sql = "SELECT Id, Name, Email FROM dbo.V_User";
        var users = await db.QueryAsync<User>(sql);

        return TypedResults.Ok(users);
    }

    // IDで1件取得
    public static async Task<Results<Ok<User>, NotFound>> GetUserById(int id, IDbConnection db)
    {
        var sql = "SELECT Id, Name, Email FROM dbo.V_User WHERE Id = @Id";
        // QuerySingleOrDefaultAsync は、見つからなければ null を返すぜ
        var user = await db.QuerySingleOrDefaultAsync<User>(sql, new { Id = id });

        return user is not null
            ? TypedResults.Ok(user)
            : TypedResults.NotFound();
    }

    // 新規作成
    public static async Task<Created<User>> CreateUser(CreateUserRequest request, IDbConnection db)
    {
        // SQL Server特有の「OUTPUT INSERTED.Id」を使って、採番されたIDを即座に取得するテクニックだ！
        var sql = @"
            INSERT INTO dbo.V_User (Name, Email) 
            OUTPUT INSERTED.Id 
            VALUES (@Name, @Email)";

        // パラメータは request オブジェクトをそのまま渡せば、@Name と @Email に自動でマッピングされる
        var newId = await db.ExecuteScalarAsync<int>(sql, request);

        // 新しいIDを使ってUserレコードを生成
        var newUser = new User(newId, request.Name, request.Email);

        return TypedResults.Created($"/api/users/{newUser.Id}", newUser);
    }
}