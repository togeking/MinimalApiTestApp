using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace api.Features.Users;
public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        // 共通のURLプレフィックスとタグを付ける
        var group = app.MapGroup("/api/users")
                        .WithTags("Users");

        // 処理自体は UserHandlers クラスに丸投げ！
        group.MapGet("/", UserHandlers.GetAllUsers);
        group.MapGet("/{id}", UserHandlers.GetUserById);
        group.MapPost("/", UserHandlers.CreateUser);
    }
}