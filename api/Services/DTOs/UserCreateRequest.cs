namespace api.Services.DTOs;

// 入力用：作成時に必要な情報だけをパッケージする
public record UserCreateRequest(string Name, string Email);