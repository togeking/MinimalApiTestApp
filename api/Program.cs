using Microsoft.Data.SqlClient;
using System.Data;
using api.Repositories;
using api.Repositories.Infrastructure;
using api.Services;
using Dapper;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. 設定の読み込みとDI登録
// ==========================================

// CORSの設定
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLiveServer", policy =>
    {
        // 💡 Live Server のURLを完全一致で指定する。末尾の「/」は入れちゃダメ！
        policy.WithOrigins("http://127.0.0.1:5500", "http://localhost:5500") 
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// appsettings.json (提示された app.json) から ConnectionStrings:DefaultConnection を取得
// builder.Configuration はデフォルトで appsettings.json を見に行くぜ
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// DateOnly型のハンドラを登録
SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());

// IDbConnection の登録
// SqlConnection を使うために Microsoft.Data.SqlClient の using が必要だ
builder.Services.AddTransient<IDbConnection>(sp => new SqlConnection(connectionString));

// 各レイヤーの登録
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<ITodoRepository, TodoRepository>();
builder.Services.AddScoped<ITodoService, TodoService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
// SwaggerGenの設定を拡張
builder.Services.AddSwaggerGen(options =>
{
    // 実行中のアセンブリ（プロジェクト）の名前からXMLファイル名を特定する
    var xmlFilename = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    
    // SwaggerにXMLドキュメントを読み込ませる
    options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// ==========================================
// 2. ミドルウェアの設定
// ==========================================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowLiveServer");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();