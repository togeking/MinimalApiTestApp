namespace api.Features.Users
{
    public record User(int Id, string Name, string Email);

    public record CreateUserRequest(string Name, string Email);
}