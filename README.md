# MinimalAPITestApp

このプロジェクトは、従来の重量級 ORM の代わりに **Dapper** をデータアクセスに使用し、高パフォーマンスとクリーンなコードに焦点を当てた、軽量な .NET Minimal API のデモプロジェクトです。

## 技術スタック
- **ランタイム:** .NET 8.0 以上
- **API フレームワーク:** ASP.NET Core Minimal APIs
- **データアクセス:** [Dapper](https://github.com/DapperLib/Dapper) (Micro-ORM)
- **データベース:** SQL Server

## プロジェクト構造
アプリケーションの規模が拡大してもメンテナンス性を維持できるよう、**機能単位（Feature-based）のフォルダ構造**を採用しています。

```text
api/
├── Features/
│   └── Users/
│       ├── User.cs          # ユーザーモデル (POCO)
│       └── UserHandlers.cs  # API エンドポイントロジック (Dapper による実装)
├── Program.cs               # アプリケーションの起動設定とルーティング
└── api.csproj
```

## はじめに（Getting Started）

### 事前準備
- .NET SDK
- SQL Server インスタンス

### データベースのセットアップ
本アプリケーションは、`dbo.V_User` という名前のテーブルまたはビューが存在することを前提としています。データベースのスキーマが以下と一致していることを確認してください。

```sql
CREATE TABLE dbo.V_User (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100),
    Email NVARCHAR(255)
);
```

### アプリケーションの実行方法
1. `api` ディレクトリに移動します。
2. `appsettings.json` で接続文字列を設定します（`Program.cs` で `IDbConnection` が正しく登録されていることを確認してください）。
3. 以下のコマンドを実行します：
   ```
   dotnet run
   ```

## API エンドポイント

| メソッド | エンドポイント | 説明 |
| :--- | :--- | :--- |
| GET | `/api/users` | すべてのユーザーを取得します |
| GET | `/api/users/{id}` | 指定した ID のユーザーを取得します |
| POST | `/api/users` | 新しいユーザーを作成します (201 Created を返します) |