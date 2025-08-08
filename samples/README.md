# deps.json 実践サンプル

このディレクトリには、deps.jsonの理解を深めるための実践的なサンプルコードが含まれています。

## サンプル一覧

1. **基本的なdeps.json構造の確認**
   - シンプルなコンソールアプリケーション
   - deps.jsonファイルの基本構造を理解

2. **複数依存関係の管理**
   - NuGetパッケージを複数使用
   - 推移的依存関係の確認

3. **プラットフォーム固有の依存関係**
   - ネイティブライブラリの依存関係
   - RID（Runtime Identifier）の影響

4. **パフォーマンス測定**
   - deps.jsonありなしでの起動時間比較
   - アセンブリロード時間の測定

## 実行手順

### 1. プロジェクトのビルド

```bash
# ソリューション全体をビルド
dotnet build

# 特定のプロジェクトをビルド
dotnet build src/DepsJsonDemo/DepsJsonDemo.csproj
```

### 2. アプリケーションの実行

```bash
# 開発環境での実行
dotnet run --project src/DepsJsonDemo/DepsJsonDemo.csproj

# 公開版での実行（deps.jsonが生成される）
dotnet publish src/DepsJsonDemo/DepsJsonDemo.csproj -o ./publish
./publish/DepsJsonDemo
```

### 3. deps.json解析ツールの使用

```bash
# DependencyAnalyzerをビルド
dotnet build src/DependencyAnalyzer/DependencyAnalyzer.csproj

# deps.jsonファイルを解析
dotnet run --project src/DependencyAnalyzer/DependencyAnalyzer.csproj -- \
  --file ./publish/DepsJsonDemo.deps.json --verbose
```

## 期待される出力例

### DepsJsonDemo実行結果

```
=== .NET deps.json 学習デモ ===

--- アプリケーション情報 ---
Assembly名: DepsJsonDemo
バージョン: 1.0.0.0
場所: /path/to/DepsJsonDemo.dll
実行時フレームワーク: .NET 8.0.0

--- deps.json 解析 ---
deps.jsonファイルが見つかりました: /path/to/DepsJsonDemo.deps.json
deps.json の主要セクション:
  - runtimeTarget
  - compilationOptions
  - targets
  - libraries

ランタイムターゲット:
  名前: .NETCoreApp,Version=v8.0

依存ライブラリ数: 15

--- 共通フレームワークのデモ ---
共通フレームワーク情報:
  名前: CommonFramework
  バージョン: 1.0.0
  場所: /path/to/CommonFramework.dll

処理結果: {"Input":"サンプルデータ","ProcessedAt":"2024-01-01T12:00:00Z","Framework":{"Name":"CommonFramework","Version":"1.0.0","Location":"/path/to/CommonFramework.dll","FullName":"CommonFramework, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"}}
```

### DependencyAnalyzer実行結果

```
=== deps.json解析結果: DepsJsonDemo.deps.json ===

--- 基本情報 ---
ランタイムターゲット: .NETCoreApp,Version=v8.0

--- ランタイム情報 ---
ターゲット: .NETCoreApp,Version=v8.0
  ライブラリ数: 15

--- ライブラリ情報 ---
総ライブラリ数: 15
NuGetパッケージ: 12
プロジェクト参照: 1

NuGetパッケージ一覧:
  - Newtonsoft.Json/13.0.3
    SHA512: HjXukE4iGYHp6...
    パス: newtonsoft.json/13.0.3
  - System.Text.Json/8.0.0
    SHA512: qA4fB8iXK5nv2...
    パス: system.text.json/8.0.0

プロジェクト参照一覧:
  - CommonFramework/1.0.0
    パス: ../CommonFramework/CommonFramework.csproj
```

## 学習ポイント

### 1. deps.json生成タイミング

- **dotnet build**: 基本的なdeps.jsonを生成（開発用）
- **dotnet publish**: 完全なdeps.jsonを生成（配布用）

### 2. 依存関係解決メカニズム

- NuGetパッケージの推移的依存関係が自動解決
- バージョン競合時の解決ルール
- フレームワーク依存関係の処理

### 3. パフォーマンス影響

- deps.jsonがないとランタイムでアセンブリ探索が発生
- 事前解決により起動時間が短縮
- メモリ使用量の最適化

### 4. トラブルシューティング

- 依存関係の欠損検出
- バージョン競合の特定
- プラットフォーム固有問題の診断

## 次のステップ

1. **異なるターゲットフレームワークでの動作確認**
2. **Self-contained deploymentでの動作確認**
3. **カスタムNuGetパッケージの作成と依存関係確認**
4. **GitHub Actionsでの自動テスト設定**
