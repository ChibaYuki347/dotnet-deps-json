# 🎯 .NET deps.json 包括的学習プロジェクト

> **.NET依存関係管理の完全理解** - 基礎から企業級アプリケーションまで

このプロジェクトは.NET アプリケーションの**deps.json ファイル**について、理論的理解から実践的応用まで包括的に学習するためのリソースです。実際のコードを通じて、.NETの依存関係管理システムの深部を探求します。

## 🎯 学習目標達成マップ

### ✅ **基礎レベル** - deps.jsonの理解
- [x] deps.jsonファイルの構造と役割
- [x] Framework-dependent vs Self-contained の違い  
- [x] プロジェクト参照 vs NuGetパッケージ参照

### ✅ **実践レベル** - 実際の開発での応用
- [x] バージョン競合の検出と解決
- [x] 依存関係解析ツールの作成
- [x] パフォーマンス影響の測定

### ✅ **応用レベル** - 企業級開発
- [x] カスタムNuGetパッケージ作成
- [x] GitHub Packages統合
- [x] CI/CD自動化パイプライン

### 🔄 **高度レベル** - エキスパート活用（実験中）
- [ ] 複雑な依存関係グラフでの競合解決
- [ ] パフォーマンスベンチマークと最適化
- [ ] Enterprise-grade分析ツールの構築

## 📚 学習ドキュメント体系

### 📋 **基礎知識ドキュメント**

| ドキュメント | 目的 | 対象読者 | 実験結果 |
|-------------|------|----------|----------|
| **[deps-json-overview.md](docs/deps-json-overview.md)** | deps.jsonの基本構造と役割の理解 | 初心者〜中級者 | 基礎実験 |
| **[final-learning-report.md](docs/final-learning-report.md)** | 基礎学習フェーズの完全な成果記録 | 全レベル | 初期〜中期実験 |

### 🔬 **実験記録ドキュメント**

| ドキュメント | 実験内容 | 主な発見 | データ |
|-------------|----------|----------|--------|
| **[practical-analysis-results.md](docs/practical-analysis-results.md)** | Self-contained Deployment実験<br>バージョン競合解決実験<br>NuGetパッケージ作成実験 | • deps.jsonサイズ34.5倍変化<br>• バージョン競合解決メカニズム<br>• type: project vs package | 定量的分析 |
| **[samples/execution-log.md](samples/execution-log.md)** | 実際の実行ログと出力結果の詳細記録 | リアルタイム実行結果 | 実行データ |

### 🚀 **応用・実践ガイド**

| ドキュメント | 応用分野 | 実装レベル | 実用性 |
|-------------|----------|------------|--------|
| **[nuget-github-guide.md](docs/nuget-github-guide.md)** | GitHub Packages統合とCI/CD | 実装完了 | 企業適用可能 |
| **[advanced-learning-plan.md](docs/advanced-learning-plan.md)** | 4つの高度トピックの詳細学習計画 | 計画・一部実装 | 研究開発向け |

## 🏗️ プロジェクト構成と実際の成果

```text
dotnet-deps-json/
├── src/                          # 🎯 実際のコードとツール
│   ├── DepsJsonDemo/            # ✅ メインデモ（Newtonsoft.Json統合済み）
│   ├── CommonFramework/         # ✅ NuGetパッケージ化済み（GitHub Packages対応）
│   └── DependencyAnalyzer/      # ✅ 実動するdeps.json解析ツール
├── docs/                        # 📚 学習ドキュメント体系
│   ├── deps-json-overview.md    # 基礎理論ドキュメント
│   ├── final-learning-report.md # 基礎学習完了レポート
│   ├── practical-analysis-results.md # 高度実験結果
│   ├── nuget-github-guide.md    # GitHub Packages実践ガイド
│   └── advanced-learning-plan.md # 研究開発計画
├── samples/                     # 📊 実験データとログ
│   └── execution-log.md         # 実際の実行記録
├── publish-result/              # 🚀 Framework-dependent配布
│   └── DepsJsonDemo.deps.json   # (787 bytes - 基本形)
├── publish-scd/                 # 📦 Self-contained配布  
│   └── DepsJsonDemo.deps.json   # (27,167 bytes - 34.5倍)
├── .github/workflows/           # ⚙️ CI/CD自動化（実装済み）
└── tests/                       # 🧪 テストスイート
```

## 🚀 クイックスタート - レベル別実行ガイド

### 🔰 **レベル1: 基礎体験**（5分）

```bash
# プロジェクトクローン
git clone https://github.com/ChibaYuki347/dotnet-deps-json.git
cd dotnet-deps-json

# 基本動作確認
dotnet build
dotnet run --project src/DepsJsonDemo
```

### 🔧 **レベル2: 詳細分析**（15分）

```bash
# deps.json詳細解析
dotnet run --project src/DependencyAnalyzer -- --file src/DepsJsonDemo/bin/Debug/net9.0/DepsJsonDemo.deps.json --verbose

# Self-contained vs Framework-dependent比較
ls -la publish-result/DepsJsonDemo.deps.json
ls -la publish-scd/DepsJsonDemo.deps.json
```

### 🏢 **レベル3: 企業級活用**（30分）

```bash
# NuGetパッケージ作成
dotnet pack src/CommonFramework/CommonFramework.csproj -c Release

# GitHub Actionsパイプライン確認
cat .github/workflows/build.yml
cat .github/workflows/publish.yml
```

## 📊 実証された学習成果データ

### 🔍 **定量的分析結果**

| 指標 | Framework-dependent | Self-contained | 変化率 |
|------|-------------------|----------------|--------|
| **deps.jsonサイズ** | 787 bytes | 27,167 bytes | **+3,452%** |
| **deps.json行数** | 38行 | 797行 | **+2,097%** |
| **配布ファイル数** | 8個 | 192個 | **+2,400%** |
| **総配布サイズ** | ~50KB | ~32MB | **+64,000%** |

### 🎯 **実証されたコンセプト**

1. **依存関係解決アルゴリズム**: バージョン競合NU1605エラーの実体験と解決
2. **プロジェクト参照 vs パッケージ参照**: `type: "project"` → `type: "package"`の変化確認
3. **Enterprise配布パイプライン**: GitHub Packages + Actions自動化の実装完了

## 🎓 学習レベル別推奨パス

### 👶 **初心者** (.NET deps.json初心者)
1. `docs/deps-json-overview.md` で基本概念理解
2. `src/DepsJsonDemo` 実行で実際の動作確認
3. `samples/execution-log.md` で実行結果の読み方学習

### 👨‍💻 **中級者** (.NET開発者)
1. `src/DependencyAnalyzer` で解析ツール活用
2. `docs/practical-analysis-results.md` で実験結果深堀り
3. 自分のプロジェクトでdeps.json分析実践

### 🏢 **上級者** (アーキテクト・DevOps)
1. `docs/nuget-github-guide.md` で企業配布実装
2. `docs/advanced-learning-plan.md` で研究開発計画確認
3. CI/CDパイプライン (`/.github/workflows/`) カスタマイズ

### 🔬 **研究者** (パフォーマンス・セキュリティ専門家)
1. `publish-scd/` の全192ファイル分析
2. Self-contained deploymentのパフォーマンス測定
3. 大規模アプリケーションでのベンチマーク実施

## 🌟 このプロジェクトの価値

### � **学習リソースとして**
- **段階的学習**: 基礎→実践→応用→研究の明確な学習パス
- **実際のデータ**: 理論だけでなく実測値による裏付け
- **再現可能**: 全ての実験が再実行可能な環境

### 🛠️ **実践ツールとして**
- **即戦力**: DependencyAnalyzerは実際のプロジェクトで使用可能
- **テンプレート**: GitHub Packagesパイプラインをそのまま利用可能
- **ベンチマーク**: パフォーマンス比較の基準値として活用

### 🏢 **企業開発として**
- **配布戦略**: Framework-dependent vs Self-containedの選択基準
- **DevOps**: CI/CD自動化の実装例
- **セキュリティ**: SHA512ハッシュ検証の実装パターン

## 🤝 コントリビューション

このプロジェクトは**学習コミュニティ**として成長させたいと考えています：

- 🐛 **バグ報告**: issueでの報告歓迎
- 📝 **ドキュメント改善**: より分かりやすい説明の提案
- 🧪 **新しい実験**: 追加の deps.json 実験アイデア  
- 🏢 **企業事例**: 実際の適用事例の共有

## 📞 サポート

- **GitHub Issues**: 技術的な質問
- **GitHub Discussions**: 学習相談・アイデア交換
- **Pull Requests**: 改善提案

## 🏷️ タグ

`dotnet` `deps-json` `dependency-management` `nuget` `github-packages` `self-contained-deployment` `framework-dependent` `performance-analysis` `devops` `ci-cd` `learning-resources` `enterprise-development` `csharp` `dotnet-core`

---

**🎯 目標**: .NET deps.jsonエコシステムの包括的理解と実践的活用の実現  
**📈 現在の進捗**: 基礎〜応用レベル完了、高度レベル実験中  
**🚀 次のマイルストーン**: Enterprise-grade分析ツールの完成
