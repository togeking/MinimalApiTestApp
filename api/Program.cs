using Microsoft.Data.SqlClient;
using System.Data;
using api.Repositories;
using api.Services;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. 設定の読み込みとDI登録
// ==========================================

// appsettings.json (提示された app.json) から ConnectionStrings:DefaultConnection を取得
// builder.Configuration はデフォルトで appsettings.json を見に行くぜ
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// IDbConnection の登録
// SqlConnection を使うために Microsoft.Data.SqlClient の using が必要だ
builder.Services.AddTransient<IDbConnection>(sp => new SqlConnection(connectionString));

// 各レイヤーの登録
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ==========================================
// 2. ミドルウェアの設定
// ==========================================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();