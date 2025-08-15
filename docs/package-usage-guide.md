# 📦 CommonFramework パッケージ利用ガイド

## 🚀 インストール方法

### 1. GitHub Packagesからのインストール

```bash
# プロジェクトにパッケージを追加
dotnet add package CommonFramework --source "https://nuget.pkg.github.com/ChibaYuki347/index.json"

# 特定バージョンを指定
dotnet add package CommonFramework --version 1.0.0 --source "https://nuget.pkg.github.com/ChibaYuki347/index.json"
```

### 2. nuget.configでの設定

プロジェクトルートに `nuget.config` ファイルを作成：

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
    <add key="github-chibayuki347" value="https://nuget.pkg.github.com/ChibaYuki347/index.json" />
  </packageSources>
</configuration>
```

### 3. 認証設定

プライベートパッケージの場合、GitHub Personal Access Tokenが必要：

```bash
dotnet nuget add source "https://nuget.pkg.github.com/ChibaYuki347/index.json" \
  --name "github-chibayuki347" \
  --username "YOUR_GITHUB_USERNAME" \
  --password "YOUR_GITHUB_TOKEN" \
  --store-password-in-clear-text
```

## 📋 使用例

### 基本的な使用方法

```csharp
using CommonFramework;

// CommonFrameworkの機能を使用
var service = new SomeService();
var result = service.ProcessData("example");
Console.WriteLine($"結果: {result}");
```

### deps.json分析での利用

```csharp
// deps.jsonファイル内でCommonFrameworkの参照を確認
// パッケージ参照として以下のような構造が生成されます：

{
  "runtimeTarget": {
    "name": ".NETCoreApp,Version=v9.0",
    "signature": ""
  },
  "compilationOptions": {},
  "targets": {
    ".NETCoreApp,Version=v9.0": {
      "CommonFramework/1.0.0": {
        "type": "package",
        "serviceable": true,
        "sha512": "...",
        "dependencies": {},
        "compile": {
          "lib/net9.0/CommonFramework.dll": {}
        },
        "runtime": {
          "lib/net9.0/CommonFramework.dll": {}
        }
      }
    }
  },
  "libraries": {
    "CommonFramework/1.0.0": {
      "type": "package",
      "serviceable": true,
      "sha512": "...",
      "path": "commonframework/1.0.0",
      "hashPath": "commonframework.1.0.0.nupkg.sha512"
    }
  }
}
```

## 🔍 利用可能なバージョン

最新のリリース情報は以下で確認できます：

- [リリースページ](https://github.com/ChibaYuki347/dotnet-deps-json/releases)
- [パッケージページ](https://github.com/ChibaYuki347/dotnet-deps-json/packages)

## 📊 deps.json変化の分析

### プロジェクト参照 vs パッケージ参照

#### プロジェクト参照の場合

- `type: "project"`
- `serviceable: false`
- SHA512ハッシュなし
- 相対パス参照

#### パッケージ参照の場合

- `type: "package"`
- `serviceable: true`
- SHA512ハッシュ付き
- NuGetキャッシュパス

### 分析ツールでの確認

```bash
# DependencyAnalyzerを使用してdeps.jsonを分析
dotnet run --project path/to/DependencyAnalyzer -- --file YourApp.deps.json --verbose
```

## 🛠️ トラブルシューティング

### よくある問題と解決方法

1. **認証エラー**

   ```text
   error NU1301: Unable to load the service index for source
   ```

   → GitHub Personal Access Tokenを確認

2. **パッケージが見つからない**

   ```text
   error NU1101: Unable to find package
   ```

   → ソース設定とバージョン指定を確認

3. **権限エラー**

   ```text
   error NU1301: Unauthorized
   ```

   → リポジトリへのアクセス権限を確認

## 📈 パフォーマンス情報

### deps.jsonファイルサイズの変化

- **プロジェクト参照時**: 約3つのライブラリエントリ
- **パッケージ参照時**: 約34つのライブラリエントリ
- **セキュリティ向上**: SHA512ハッシュ検証の追加

## 🤝 コントリビューション

パッケージの改善提案やバグ報告は [Issues](https://github.com/ChibaYuki347/dotnet-deps-json/issues) までお願いします。

## 📄 ライセンス

MIT License - 詳細は [LICENSE](LICENSE) ファイルを参照してください。
