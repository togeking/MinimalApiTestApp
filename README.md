# My Todo App (Fullstack Architecture)

このプロジェクトは、フロントエンドに Vanilla JS、バックエンドに .NET 8 (Controllerベース) を採用した、実践的なフルスタック Todo アプリケーションです。
従来の重量級 ORM の代わりに **Dapper** をデータアクセスに使用し、高パフォーマンスと「3層アーキテクチャ」によるクリーンなコード分離に焦点を当てています。

## 🛠 技術スタック

**Backend (`/api`)**
- **ランタイム:** .NET 8.0
- **アーキテクチャ:** 3層アーキテクチャ (Controllers, Services, Repositories)
- **データアクセス:** Dapper (Micro-ORM)
- **データベース:** SQL Server
- **API ドキュメント:** Swagger (開発環境のみ)

**Frontend (`/ui`)**
- **UI:** HTML5, CSS3, Vanilla JS
- **モジュール管理:** ES Modules (`type="module"`)
- **HTTP クライアント:** Axios

---

## 📂 プロジェクト構造

「関心の分離」を徹底するため、バックエンドは役割ごとにレイヤー分けされ、フロントエンドも通信と描画を分離しています。

```text
MinimalApiTestApp/
├── api/                  # バックエンド (C# / .NET 8)
│   ├── Controllers/      # 窓口: HTTPリクエストの受付とレスポンス返却
│   ├── Services/         # 司令塔: ビジネスロジックと DTO ⇔ Entity の変換
│   ├── Repositories/     # 倉庫番: Dapperを用いた SQL Server との通信
│   ├── Program.cs        # DIコンテナ、CORS、Swaggerなどの構成
│   └── api.csproj
└── ui/                   # フロントエンド (Vanilla JS)
    ├── index.html        # アプリケーションの骨組み
    └── assets/
        ├── css/
        │   └── style.css # スタイリング
        └── js/
            ├── api.js    # 通信兵: Axiosを用いた API との通信処理
            └── app.js    # 現場監督: DOMの操作とイベントハンドリング
```

---

## 🚀 はじめに（Getting Started）

### 1. 事前準備
- .NET 8 SDK
- SQL Server インスタンス
- VS Code (推奨拡張機能: Live Server, C#)

### 2. データベースのセットアップ
本アプリケーションは、以下のテーブル構成が存在することを前提としています。SQL Server で以下のスクリプトを実行してください。

```sql
-- ユーザー管理テーブル
CREATE TABLE dbo.V_User (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) NOT NULL,
    CreatedAt DATETIME DEFAULT GETUTCDATE()
);

-- Todo管理テーブル
CREATE TABLE dbo.V_Todo (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(200) NOT NULL,
    UserId INT NOT NULL,
    AssigneeId INT NOT NULL,
    StartDate DATE NULL,
    EndDate DATE NULL,
    Status INT NOT NULL DEFAULT 0, -- 0: NotStarted, 1: InProgress, 2: Completed
    CreatedAt DATETIME DEFAULT GETUTCDATE()
);
```

### 3. アプリケーションの起動（VS Code フルスタックデバッグ）
本プロジェクトは、VS Code の `launch.json` に **API と UI の同時起動設定** が組み込まれています。

1. `api/appsettings.json` に正しい DB 接続文字列 (`DefaultConnection`) を設定します。
2. VS Code でプロジェクトのルートフォルダを開きます。
3. デバッグパネルから `🚀 Fullstack Debug (API + UI)` を選択し、**F5キー** を押します。
4. バックエンドが立ち上がり、自動的にブラウザでフロントエンド (`http://127.0.0.1:5500/index.html`) が開きます。

*(※フロントエンドの配信には、VS Codeの拡張機能「Live Server」がバックグラウンドで起動している必要があります。)*

---

## 📡 API エンドポイント一覧

Swagger UI は、API 起動中に `https://localhost:<port>/swagger` へアクセスすることで確認・テストが可能です。

### Users API (`/api/Users`)
| メソッド | エンドポイント | 説明 |
| :--- | :--- | :--- |
| `GET` | `/api/Users` | すべてのユーザーを取得します |
| `GET` | `/api/Users/{id}` | 指定した ID のユーザーを取得します |
| `POST` | `/api/Users` | 新しいユーザーを作成します |
| `DELETE`| `/api/Users/{id}` | 指定したユーザーを削除します |

### Todos API (`/api/Todos`)
| メソッド | エンドポイント | 説明 |
| :--- | :--- | :--- |
| `GET` | `/api/Todos` | すべてのタスクを取得します |
| `GET` | `/api/Todos/{id}` | 指定した ID のタスクを取得します |
| `GET` | `/api/Todos/user/{userId}` | 特定のユーザーのタスクを取得します |
| `POST` | `/api/Todos` | 新しいタスクを作成します |
| `PUT` | `/api/Todos/{id}` | 既存のタスクを更新します |
| `DELETE`| `/api/Todos/{id}` | 指定したタスクを削除します |

---

## 💡 設計上の工夫 (Architectural Notes)

- **CORS対応**: `Program.cs` にて、フロントエンド (Live Server) からの通信を許可する設定が組み込まれています。
- **型変換の最適化**: .NET 8 の `DateOnly` 型を Dapper で扱うため、専用の `DateOnlyTypeHandler` を実装し、DB の `DATE` 型とシームレスにマッピングしています。
- **DTOの活用**: クライアントに見せるデータ (DTO) と DB に保存するデータ (Entity) を厳密に分けることで、将来の仕様変更に強い設計にしています。