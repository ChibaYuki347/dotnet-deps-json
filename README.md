# .NET deps.json 学習プロジェクト

このプロジェクトは.NET の deps.json ファイルの効果と使い方を理解するためのサンプルコードです。
また、共通フレームワークを NuGet パッケージとして管理し、GitHub で運用する方法についても学習できます。

## 🎯 学習目標

- **deps.json ファイルの仕組みと役割の理解**
- **依存関係管理のメカニズム学習**
- **NuGet パッケージ作成と GitHub 管理**
- **実際のアプリケーションでの動作確認**

## 📁 プロジェクト構成

```text
dotnet-deps-json/
├── src/
│   ├── DepsJsonDemo/              # メインデモアプリケーション
│   ├── CommonFramework/           # 共通フレームワークライブラリ（NuGetパッケージ化）
│   └── DependencyAnalyzer/        # deps.json解析ツール
├── samples/                       # サンプルコードとドキュメント
├── docs/                         # 詳細ドキュメント
├── tests/                        # テストプロジェクト
└── .github/workflows/            # CI/CDパイプライン
```

## 🚀 クイックスタート

### 1. プロジェクトのクローンとビルド

```bash
git clone https://github.com/ChibaYuki347/dotnet-deps-json.git
cd dotnet-deps-json
dotnet build
```

### 2. デモアプリケーションの実行

```bash
# 開発環境での実行
dotnet run --project src/DepsJsonDemo/DepsJsonDemo.csproj

# 本番環境用ビルドと実行
dotnet publish src/DepsJsonDemo/DepsJsonDemo.csproj -o ./publish
./publish/DepsJsonDemo
```

### 3. deps.json 解析ツールの使用

```bash
# 詳細解析の実行
dotnet run --project src/DependencyAnalyzer/DependencyAnalyzer.csproj -- \
  --file ./publish/DepsJsonDemo.deps.json --verbose
```

## 📚 deps.json とは

deps.json ファイルは.NET Core/.NET 5+アプリケーションの**依存関係管理システム**の中核となるファイルです。

### 主な役割

- **ランタイム依存関係の管理**: アプリケーションが実行時に必要なアセンブリの情報
- **ネイティブライブラリの解決**: プラットフォーム固有のネイティブライブラリの場所
- **パッケージの依存関係グラフ**: NuGet パッケージ間の依存関係
- **アセンブリロードの最適化**: ランタイムでのアセンブリ検索の高速化

### 実用的な効果

1. **高速なアプリケーション起動** - 事前に依存関係が解決済み
2. **確実な依存関係管理** - バージョン競合の回避
3. **セキュリティの向上** - ハッシュ値による整合性チェック

## 🛠️ プロジェクト詳細

### DepsJsonDemo（メインアプリケーション）

deps.json ファイルの効果を実際に確認できるコンソールアプリケーションです。

**主な機能:**

- アプリケーション情報の表示
- deps.json ファイルの構造解析
- 共通フレームワークとの連携デモ
- 依存関係の動的解析

### CommonFramework（共通ライブラリ）

NuGet パッケージとして配布される共通フレームワークのサンプルです。

**特徴:**

- NuGet パッケージ設定の実装例
- deps.json での依存関係管理
- GitHub Packages での配布準備

### DependencyAnalyzer（解析ツール）

deps.json ファイルを詳細に解析して、依存関係情報を可視化するツールです。

**機能:**

- deps.json ファイルの構造解析
- 依存関係ツリーの表示
- パッケージ統計情報の提供
- 詳細モードでの完全解析

## 📖 学習の進め方

### レベル 1: 基本理解

1. **[deps.json 概要ドキュメント](docs/deps-json-overview.md)** を読む
2. **DepsJsonDemo** を実行して基本動作を確認
3. **deps.json ファイル** の内容を直接確認

### レベル 2: 実践的な使用

1. **DependencyAnalyzer** で詳細解析を実行
2. **自分のプロジェクト** で deps.json を確認
3. **異なる依存関係** での deps.json 比較

### レベル 3: 高度な活用

1. **[NuGet 管理ガイド](docs/nuget-github-guide.md)** を参考にパッケージ作成
2. **GitHub Actions** での CI/CD 設定
3. **カスタム NuGet サーバー** の構築

## 🔧 開発環境

- **.NET 8.0 以上**
- **Visual Studio 2022** / **VS Code** / **JetBrains Rider**
- **Git**

## 📝 ドキュメント

詳細なドキュメントは`docs/`ディレクトリにあります：

- **[deps.json 詳細ガイド](docs/deps-json-overview.md)**
- **[NuGet と GitHub 管理](docs/nuget-github-guide.md)**

## 🤝 コントリビューション

1. このリポジトリをフォーク
2. フィーチャーブランチを作成 (`git checkout -b feature/amazing-feature`)
3. 変更をコミット (`git commit -m 'Add amazing feature'`)
4. ブランチにプッシュ (`git push origin feature/amazing-feature`)
5. プルリクエストを作成

## 📄 ライセンス

このプロジェクトは MIT ライセンスのもとで公開されています。詳細は [LICENSE](LICENSE) ファイルを参照してください。

## 🏷️ タグ

`dotnet` `deps-json` `nuget` `dependency-management` `github-packages` `csharp` `learning-resources`
