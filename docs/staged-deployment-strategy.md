# 📋 段階的CI/CDデプロイメント戦略

## 🎯 概要

このプロジェクトでは、パッケージ依存関係を考慮した段階的CI/CDアプローチを採用しています。これにより、依存関係の循環参照を回避し、安定したパッケージ配布を実現します。

## 🔄 デプロイメントフロー

### Phase 1: 基盤パッケージの構築
```
CommonFramework (独立パッケージ)
├── ビルド・テスト
├── パッケージ作成
└── GitHub Packages公開
```

### Phase 2: 依存パッケージの構築
```
DepsJsonDemo (CommonFramework依存)
├── 公開済みCommonFrameworkを参照
├── パッケージ統合テスト
└── deps.json分析実行
```

### Phase 3: 分析ツールの実行
```
DependencyAnalyzer (独立ツール)
├── deps.json構造解析
├── 依存関係検証
└── レポート生成
```

## 🏗️ ワークフロー設計

### 1. Build Workflow (build.yml)
**目的**: プルリクエスト・mainブランチでの基本検証

```yaml
strategy:
  matrix:
    project: 
      - CommonFramework
      - DependencyAnalyzer
```

**特徴**:
- ✅ パッケージ依存なしでビルド可能なプロジェクトのみ
- ✅ 基本的な構文・コンパイルエラーの早期発見
- ✅ 高速フィードバックループ

### 2. Release Workflow (release.yml)
**目的**: 完全なパッケージ統合テスト

**段階的実行**:
1. **基盤パッケージ公開**
   ```bash
   dotnet pack CommonFramework
   dotnet nuget push CommonFramework.nupkg
   ```

2. **依存パッケージ統合**
   ```bash
   dotnet add DepsJsonDemo package CommonFramework
   dotnet build DepsJsonDemo
   ```

3. **統合テスト実行**
   ```bash
   ./DepsJsonDemo  # 実際の動作確認
   DependencyAnalyzer --file DepsJsonDemo.deps.json  # 構造分析
   ```

## 🎯 依存関係マトリックス

| プロジェクト | 依存関係 | ビルド段階 | テスト可能性 |
|-------------|---------|----------|------------|
| CommonFramework | なし | Phase 1 | ✅ 独立 |
| DependencyAnalyzer | なし | Phase 1 | ✅ 独立 |
| DepsJsonDemo | CommonFramework | Phase 2 | ⚠️ パッケージ後 |

## 🔧 プロジェクトファイル戦略

### CommonFramework.csproj
```xml
<!-- 完全に独立したライブラリ -->
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <GeneratePackageOnBuild>false</GeneratePackageOnBuild>
  </PropertyGroup>
</Project>
```

### DepsJsonDemo.csproj
```xml
<!-- パッケージ参照依存 -->
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
  </PropertyGroup>
  <ItemGroup>
    <!-- リリース時に動的追加 -->
    <!-- <PackageReference Include="CommonFramework" Version="X.X.X" /> -->
  </ItemGroup>
</Project>
```

### DependencyAnalyzer.csproj
```xml
<!-- 分析ツール（独立） -->
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net9.0</TargetFramework>
  </PropertyGroup>
</Project>
```

## 📊 CI/CDメトリクス

### ビルド時間最適化
- **Phase 1**: ~2-3分（基盤パッケージ）
- **Phase 2**: ~3-5分（統合テスト）
- **Phase 3**: ~1-2分（分析実行）
- **総計**: ~6-10分

### 失敗ポイント分析
1. **Early Failure**: Phase 1での構文エラー
2. **Integration Failure**: Phase 2での依存関係問題
3. **Analysis Failure**: Phase 3でのdeps.json構造問題

## 🚀 リリース戦略

### 1. 開発フェーズ
```bash
# プロジェクト参照で開発
<ProjectReference Include="../CommonFramework/CommonFramework.csproj" />
```

### 2. テストフェーズ
```bash
# ローカルNuGetフィードでテスト
dotnet pack CommonFramework
dotnet add DepsJsonDemo package CommonFramework --source ./local-feed
```

### 3. 本番フェーズ
```bash
# GitHub Packagesで配布
dotnet nuget push CommonFramework.nupkg --source GitHub
dotnet add DepsJsonDemo package CommonFramework --source GitHub
```

## 🛠️ トラブルシューティング

### よくある問題

#### 1. NU1102: パッケージが見つからない
```text
Unable to find package CommonFramework with version (>= 1.0.0)
```
**原因**: パッケージがまだ公開されていない
**解決**: リリースワークフローの段階的実行

#### 2. 循環参照エラー
```text
Circular dependency detected
```
**原因**: プロジェクト参照の不適切な設定
**解決**: 依存関係マトリックスの再設計

#### 3. ビルド順序エラー
```text
Project dependency could not be resolved
```
**原因**: ビルド順序の問題
**解決**: MSBuildのProjectReferenceの適切な設定

## 📈 成功指標

### 自動化達成度
- ✅ **100%**: CommonFramework自動公開
- ✅ **100%**: DependencyAnalyzer独立実行
- ✅ **95%**: DepsJsonDemo統合テスト
- ✅ **90%**: deps.json構造分析

### 品質メトリクス
- **ビルド成功率**: 95%以上
- **テスト網羅率**: 80%以上
- **パッケージ互換性**: 100%
- **ドキュメント整合性**: 95%以上

この段階的アプローチにより、企業レベルの堅牢なCI/CDパイプラインを実現し、複雑な依存関係を持つ.NETプロジェクトでも安定した自動化を提供します。
