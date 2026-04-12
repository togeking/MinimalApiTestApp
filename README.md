# .NET & Minimal API 開発セットアップガイド

Minimal APIを使ったプロジェクトを立ち上げ、スパゲッティコードを防ぐためのベストプラクティス（ファイル分割・階層化）をまとめたガイドです。

---

## 1. 環境の確認コマンド

インストールされている.NETの状態を確認するための基本コマンドです。

- **SDK（開発キット）の一覧を表示**
  ```bash
  dotnet --list-sdks
  ```
- **ランタイム（実行環境）の一覧を表示**
  ```bash
  dotnet --list-runtimes
  ```
- **環境の全詳細情報を表示（迷ったらこれ）**
  ```bash
  dotnet --info
  ```

---

## 2. SDKバージョンの切り替え

システム全体の設定は変えずに、プロジェクト（フォルダ）ごとに使うSDKのバージョンを固定する方法です。

対象のプロジェクトルートで以下のコマンドを実行し、`global.json` を生成します。（例として `8.0.100` を指定）

```bash
dotnet new globaljson --sdk-version 8.0.100
```

- **効果範囲:** このファイルが置かれたフォルダと、その下位階層すべてに適用されます。

---

## 3. プロジェクト作成の基本コンボ

ソリューションを作成し、Minimal APIプロジェクトを立ち上げ、両者を紐づける一連のフローです。

**① ソリューションの作成**
プロジェクトをまとめる大枠（ソリューション）を作成します。

```bash
dotnet new sln -n MyAwesomeApp
```

**② Minimal APIプロジェクトの作成**
余計なファイルが含まれないスッキリした `web` テンプレートを使用します。

```bash
dotnet new web -n MyMinimalApi
```

**③ プロジェクトをソリューションに追加（連携）**
作成したAPIプロジェクトをソリューションに登録し、相互に認識させます。

```bash
dotnet sln add MyMinimalApi/MyMinimalApi.csproj
```

---

## 4. Minimal APIのベストプラクティス（コード分割）

`Program.cs` の肥大化を防ぐため、役割ごとにファイルを分割する3つのルールです。

### ① データクラスの分離

データの入れ物は専用フォルダに隔離し、モダンなC#の機能である `record` を活用して簡潔に記述します。

- **Models / Entities:** データベースのテーブル用
- **DTOs:** APIの入出力用

```csharp
// Models/Todo.cs
namespace MyMinimalApi.Models;

public record Todo(int Id, string Title, bool IsCompleted);
```

### ② 汎用関数・ビジネスロジックの分離（Services）

ビジネスロジックやデータベースへのアクセス処理は、「サービス」として別クラスに切り出します。

```csharp
// Services/TodoService.cs
using MyMinimalApi.Models;

namespace MyMinimalApi.Services;

// インターフェースで設計の約束事を作る
public interface ITodoService
{
    List<Todo> GetAwesomeTodos();
}

// 実際の処理
public class TodoService : ITodoService
{
    public List<Todo> GetAwesomeTodos()
    {
        return new List<Todo> { new Todo(1, "世界を救う", false) };
    }
}
```

### ③ エンドポイントの分離（Endpoints）

`Program.cs` に直接ルーティングを書かず、**拡張メソッド**と **`MapGroup`** を使って機能ごとにAPIの窓口を分割します。

```csharp
// Endpoints/TodoEndpoints.cs
using MyMinimalApi.Services;

namespace MyMinimalApi.Endpoints;

public static class TodoEndpoints
{
    // IEndpointRouteBuilderの拡張メソッドとして定義
    public static void MapTodoEndpoints(this IEndpointRouteBuilder app)
    {
        // "/api/todos" でURLをグループ化
        var group = app.MapGroup("/api/todos");

        group.MapGet("/", GetAllTodos);
    }

    // DI（依存性の注入）を利用してサービスを呼び出す
    private static IResult GetAllTodos(ITodoService todoService)
    {
        var todos = todoService.GetAwesomeTodos();
        return Results.Ok(todos);
    }
}
```

### ④ アプリケーションの起動設定（Program.cs）

分割したクラス群を `Program.cs` で統合します。ここは「設定と登録」だけを行うクリーンな状態に保ちます。

```csharp
// Program.cs
using MyMinimalApi.Endpoints;
using MyMinimalApi.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. サービスの登録 (DI)
builder.Services.AddScoped<ITodoService, TodoService>();

var app = builder.Build();

// 2. エンドポイントの登録
app.MapTodoEndpoints();

app.Run();
```

---

## 5. 理想のフォルダ構成イメージ

最終的に、プロジェクトの構造は以下のように整理されます。

```text
MyMinimalApi/
 ┣ Models/         (データ構造: recordなど)
 ┣ DTOs/           (入出力用の一時データ)
 ┣ Endpoints/      (APIの窓口 / ルーティング)
 ┣ Services/       (ビジネスロジック / 汎用関数)
 ┣ Program.cs      (アプリの起動と設定、DI登録のみ)
 ┗ appsettings.json
```
