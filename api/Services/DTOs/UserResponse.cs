namespace api.Services.DTOs;

// 出力用：パスワードとか内部的な値は含めないのが鉄則だ
public record UserResponse(int Id, string Name, string Email);