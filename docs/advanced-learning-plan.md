# 🚀 .NET deps.json 高度学習計画

基礎学習プロジェクトの完了おめでとうございます！ここから更に深い理解を得るための4つの高度トピックについて詳しく説明します。

---

## 1. 📦 Self-contained Deployment での deps.json動作

### 概要
Self-contained Deployment（自己完結型配布）は、.NETランタイムをアプリケーションと一緒にパッケージ化して配布する方式です。このとき、deps.jsonの構造と役割が大きく変化します。

### 現在のFramework-dependent vs Self-contained の違い

**Framework-dependent（現在の構成）:**
```bash
dotnet DepsJsonDemo.dll  # .NETランタイムが必要
```

**Self-contained:**
```bash
./DepsJsonDemo           # ランタイム同梱、単体実行可能
```

### deps.jsonの変化ポイント

#### Framework-dependent の deps.json
```json
{
  "runtimeTarget": {
    "name": ".NETCoreApp,Version=v9.0"
  },
  "targets": {
    ".NETCoreApp,Version=v9.0": {
      "DepsJsonDemo/1.0.0": { ... }
    }
  }
}
```

#### Self-contained の deps.json
```json
{
  "runtimeTarget": {
    "name": ".NETCoreApp,Version=v9.0/osx-x64",
    "signature": "da39a3ee5e6b4b0d3255bfef95601890afd80709"
  },
  "targets": {
    ".NETCoreApp,Version=v9.0": { ... },
    ".NETCoreApp,Version=v9.0/osx-x64": {
      "DepsJsonDemo/1.0.0": { ... },
      "Microsoft.NETCore.App/9.0.0": {
        "runtime": {
          "System.Private.CoreLib.dll": { ... },
          "System.Runtime.dll": { ... }
          // 全てのランタイムライブラリ
        }
      }
    }
  }
}
```

### 学習実験計画

```bash
# 1. 現在のアプリをSelf-containedで配布
dotnet publish -c Release -r osx-x64 --self-contained true

# 2. deps.jsonファイルサイズと内容を比較
ls -la publish-scd/DepsJsonDemo.deps.json
ls -la publish-result/DepsJsonDemo.deps.json

# 3. 実行ファイルサイズと起動時間を測定
time ./publish-scd/DepsJsonDemo
time dotnet publish-result/DepsJsonDemo.dll
```

### 観察ポイント
- **ファイルサイズ**: deps.jsonが10-50倍大きくなる
- **ランタイム情報**: 全.NETライブラリが明示的に記録
- **プラットフォーム固有**: RID（Runtime Identifier）の追加
- **起動時間**: 初回起動が高速化される可能性

---

## 2. ⚔️ 複雑な依存関係グラフでのバージョン競合解決

### 概要
実際のアプリケーションでは複数のNuGetパッケージが同じライブラリの**異なるバージョン**を要求することがあります。.NETの依存関係解決アルゴリズムがどのように動作するかを学習します。

### バージョン競合のシナリオ例

```
あなたのアプリ
├── PackageA 1.0 → Newtonsoft.Json 12.0.3
├── PackageB 2.0 → Newtonsoft.Json 13.0.3
└── PackageC 1.5 → Newtonsoft.Json 11.0.2
```

**問題**: どのバージョンが選択される？

### 学習実験計画

#### 実験1: 単純なバージョン競合
```bash
# 意図的に異なるバージョンを追加
dotnet add package Newtonsoft.Json --version 12.0.3
dotnet add package Microsoft.Extensions.DependencyInjection --version 8.0.0
# Microsoft.Extensions.DependencyInjectionが内部でNewtonsoft.Json 13.0.3を要求
```

#### 実験2: 複雑な推移的依存関係
```bash
# 複数のパッケージを追加して競合を作成
dotnet add package Serilog.Extensions.Hosting --version 8.0.0
dotnet add package Serilog.Sinks.File --version 5.0.0
dotnet add package Microsoft.Extensions.Logging --version 7.0.0
```

#### 実験3: 分析ツール拡張
DependencyAnalyzerに以下の機能を追加：
- **バージョン競合検出**: 同じライブラリの複数バージョン特定
- **依存関係ツリー可視化**: 推移的依存関係のグラフ表示
- **解決戦略分析**: どのバージョンが選択されたかとその理由

### deps.jsonでの記録例
```json
{
  "libraries": {
    "Newtonsoft.Json/13.0.3": {
      "type": "package",
      "serviceable": true,
      "sha512": "...",
      "path": "newtonsoft.json/13.0.3"
    }
    // 12.0.3と11.0.2は記録されない（13.0.3が勝利）
  }
}
```

### 観察ポイント
- **最高バージョン原則**: 通常は最も高いバージョンが選択
- **明示的参照優先**: 直接参照が推移的参照に優先
- **Breaking Change**: 後方互換性が破られた場合の動作
- **PackageReference vs ProjectReference**: 競合解決の違い

---

## 3. 📦 カスタムNuGetパッケージ作成とGitHub Packages統合

### 概要
CommonFrameworkライブラリを実際のNuGetパッケージとして配布し、GitHub Packagesで管理します。これにより、プロジェクト参照とパッケージ参照の違いを実体験できます。

### 学習実験計画

#### Phase 1: CommonFrameworkのパッケージ化

```xml
<!-- CommonFramework.csproj の拡張 -->
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    
    <!-- NuGetパッケージメタデータ -->
    <PackageId>ChibaYuki347.CommonFramework</PackageId>
    <PackageVersion>1.0.0</PackageVersion>
    <Authors>ChibaYuki347</Authors>
    <Description>deps.json学習用共通フレームワークライブラリ</Description>
    <PackageTags>dotnet;deps.json;learning</PackageTags>
    <RepositoryUrl>https://github.com/ChibaYuki347/dotnet-deps-json</RepositoryUrl>
    
    <!-- アイコンとライセンス -->
    <PackageIcon>icon.png</PackageIcon>
    <PackageLicenseExpression>MIT</PackageLicenseExpression>
  </PropertyGroup>
</Project>
```

#### Phase 2: ローカルNuGetパッケージ作成

```bash
# パッケージ作成
dotnet pack src/CommonFramework/CommonFramework.csproj -c Release

# ローカルフィードとして追加
dotnet nuget add source ./src/CommonFramework/bin/Release --name LocalFeed

# DepsJsonDemoでプロジェクト参照をパッケージ参照に変更
dotnet remove src/DepsJsonDemo/DepsJsonDemo.csproj reference src/CommonFramework/CommonFramework.csproj
dotnet add src/DepsJsonDemo/DepsJsonDemo.csproj package ChibaYuki347.CommonFramework --version 1.0.0 --source LocalFeed
```

#### Phase 3: GitHub Packages統合

```yaml
# .github/workflows/nuget-publish.yml
name: Publish NuGet Package
on:
  push:
    tags: [ 'v*' ]

jobs:
  publish:
    runs-on: ubuntu-latest
    steps:
    - uses: actions/checkout@v4
    - name: Setup .NET
      uses: actions/setup-dotnet@v4
      with:
        dotnet-version: '9.0.x'
    
    - name: Pack
      run: dotnet pack src/CommonFramework/CommonFramework.csproj -c Release
    
    - name: Publish to GitHub Packages
      run: |
        dotnet nuget add source "https://nuget.pkg.github.com/ChibaYuki347/index.json" \
          --name GitHub --username ChibaYuki347 --password ${{ secrets.GITHUB_TOKEN }}
        dotnet nuget push "src/CommonFramework/bin/Release/*.nupkg" \
          --source GitHub --api-key ${{ secrets.GITHUB_TOKEN }}
```

### deps.json変化の観察

#### パッケージ参照後のdeps.json
```json
{
  "libraries": {
    "ChibaYuki347.CommonFramework/1.0.0": {
      "type": "package",           // project → package に変化
      "serviceable": true,         // false → true に変化
      "sha512": "ハッシュ値",      // 空 → 実際のハッシュ
      "path": "chibayuki347.commonframework/1.0.0"
    }
  }
}
```

### 観察ポイント
- **バージョン管理**: セマンティックバージョニングの実践
- **依存関係記録**: パッケージ参照がdeps.jsonにどう記録されるか
- **配布効率**: パッケージキャッシュの恩恵
- **開発ワークフロー**: パッケージ更新の手順

---

## 4. ⚡ 大規模アプリケーションでのパフォーマンス測定

### 概要
deps.jsonファイルが実際のアプリケーション起動時間とランタイムパフォーマンスに与える影響を定量的に測定します。

### パフォーマンス測定の観点

#### 1. 起動時間への影響
```csharp
// StartupBenchmark.cs
using System.Diagnostics;

public class StartupBenchmark
{
    public static void MeasureStartupTime()
    {
        var stopwatch = Stopwatch.StartNew();
        
        // deps.json読み込み時間
        var depsLoadTime = MeasureDepsJsonLoad();
        
        // アセンブリ解決時間
        var assemblyResolveTime = MeasureAssemblyResolve();
        
        // 初回JIT時間
        var jitTime = MeasureJitCompilation();
        
        stopwatch.Stop();
        
        Console.WriteLine($"Total Startup: {stopwatch.ElapsedMilliseconds}ms");
        Console.WriteLine($"deps.json Load: {depsLoadTime}ms");
        Console.WriteLine($"Assembly Resolve: {assemblyResolveTime}ms");  
        Console.WriteLine($"JIT Compilation: {jitTime}ms");
    }
}
```

#### 2. メモリ使用量の測定
```csharp
public class MemoryBenchmark
{
    public static void MeasureMemoryUsage()
    {
        var before = GC.GetTotalMemory(true);
        
        // 大量の依存関係を持つアプリケーション実行
        LoadHeavyDependencies();
        
        var after = GC.GetTotalMemory(true);
        
        Console.WriteLine($"Memory Used: {(after - before) / 1024 / 1024}MB");
    }
}
```

#### 3. 依存関係数別のパフォーマンス比較

| 依存関係数 | deps.jsonサイズ | 起動時間 | メモリ使用量 |
|------------|----------------|----------|-------------|
| 5個 (基本) | 2KB | 50ms | 20MB |
| 25個 (中程度) | 15KB | 120ms | 45MB |
| 100個 (大規模) | 80KB | 300ms | 120MB |
| 500個 (企業級) | 500KB | 800ms | 300MB |

### 学習実験計画

#### 実験1: 依存関係数による影響測定
```bash
# 段階的にパッケージを追加してベンチマーク
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package Serilog.AspNetCore
dotnet add package AutoMapper
dotnet add package FluentValidation.AspNetCore
# ... 合計50個のパッケージ
```

#### 実験2: deps.json有無での比較
```csharp
// deps.jsonを意図的に削除または破損させて起動時間を測定
File.Delete("DepsJsonDemo.deps.json");
// 実行時間とエラーハンドリングを観察
```

#### 実験3: 異なる配布方式での比較
- **Framework-dependent**: 軽量だが.NET依存
- **Self-contained**: 重いが独立実行
- **ReadyToRun**: AOTコンパイル済み
- **Native AOT**: ネイティブバイナリ

### ベンチマーキングツールの作成

```csharp
// PerformanceProfiler.cs - DependencyAnalyzerに統合
public class PerformanceProfiler
{
    public void ProfileDepsJsonImpact(string depsJsonPath)
    {
        // 1. deps.jsonファイル解析時間
        var parseTime = MeasureJsonParsing(depsJsonPath);
        
        // 2. 依存関係解決シミュレーション
        var resolveTime = SimulateDependencyResolution(depsJsonPath);
        
        // 3. メモリフットプリント分析
        var memoryImpact = AnalyzeMemoryFootprint(depsJsonPath);
        
        GeneratePerformanceReport(parseTime, resolveTime, memoryImpact);
    }
}
```

### 観察ポイント
- **deps.jsonサイズ vs 起動時間**: 線形関係か？
- **キャッシュ効果**: 2回目以降の起動時間改善
- **プラットフォーム差異**: Windows vs macOS vs Linux
- **.NETバージョン影響**: .NET 8 vs .NET 9での違い

---

## 🎯 統合学習プロジェクト提案

### "Enterprise-Grade DepsJson Analyzer"

これら4つのトピックを統合した包括的なプロジェクトを構築することをお勧めします：

#### 機能セット
1. **Multi-deployment Analysis**: Framework-dependent vs Self-contained比較
2. **Conflict Resolution Simulator**: バージョン競合の可視化と解決策提案
3. **Package Publisher**: GitHub Packages自動公開
4. **Performance Profiler**: 企業級アプリケーションのパフォーマンス分析

#### 技術スタック
- **.NET 9.0**: 最新機能活用
- **System.CommandLine**: 高度なCLI
- **GitHub Actions**: CI/CD自動化
- **BenchmarkDotNet**: 精密なパフォーマンス測定
- **Spectre.Console**: 美しいコンソール出力

---

## 🚀 次のアクション

どのトピックから始めたいでしょうか？各トピックは独立して学習できますが、以下の順序を推奨します：

1. **Self-contained Deployment** (1-2時間) - 基礎の拡張
2. **カスタムNuGetパッケージ** (2-3時間) - GitHub統合学習
3. **バージョン競合解決** (3-4時間) - 複雑な実世界問題
4. **パフォーマンス測定** (2-3時間) - 定量的分析

どのトピックに最も興味がありますか？具体的な実装を始めましょう！
