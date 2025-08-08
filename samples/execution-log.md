# deps.json学習実行ログ

このドキュメントは、.NET deps.json学習プロジェクトの実際の実行結果と分析を記録したものです。

## 📋 実行環境

- **実行日時**: 2025年8月8日 08:13-08:15 JST
- **.NET バージョン**: .NET 9.0.0
- **プラットフォーム**: macOS
- **プロジェクト**: dotnet-deps-json v1.0.0

## 🔬 実行結果と分析

### 1. プロジェクトビルド結果

```bash
$ dotnet clean && dotnet build
```

**結果:**
- ✅ **成功**: 全プロジェクトが正常にビルド
- **ビルド時間**: 2.5秒
- **生成されたアセンブリ**:
  - CommonFramework.dll (共通ライブラリ)
  - DepsJsonDemo.dll (メインアプリケーション)
  - DependencyAnalyzer.dll (解析ツール)

**学習ポイント:**
- プロジェクト間の依存関係が正しく解決されている
- .NET 9.0ターゲットで問題なくビルド完了

### 2. DepsJsonDemo実行結果（開発環境）

#### アプリケーション情報
- **Assembly名**: DepsJsonDemo
- **バージョン**: 1.0.0.0
- **実行時フレームワーク**: .NET 9.0.0
- **場所**: `/src/DepsJsonDemo/bin/Debug/net9.0/DepsJsonDemo.dll`

#### deps.json基本情報
- **ランタイムターゲット**: .NETCoreApp,Version=v9.0
- **依存ライブラリ数**: 2個（DepsJsonDemo + CommonFramework）
- **主要セクション**: runtimeTarget, compilationOptions, targets, libraries

#### 共通フレームワーク動作確認
```json
{
  "Input": "サンプルデータ",
  "ProcessedAt": "2025-08-08T08:13:50.647132Z",
  "Framework": {
    "Name": "CommonFramework",
    "Version": "1.0.0.0",
    "Location": "/src/DepsJsonDemo/bin/Debug/net9.0/CommonFramework.dll",
    "FullName": "CommonFramework, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
  }
}
```

**学習ポイント:**
- ✅ プロジェクト参照が正常に動作
- ✅ System.Text.Jsonでのシリアライゼーション成功
- ✅ Assembly情報の取得が正確

#### ロードされたアセンブリ分析

**カスタムアセンブリ（プロジェクト）:**
- CommonFramework (v1.0.0.0)
- DepsJsonDemo (v1.0.0.0)

**システムアセンブリ（.NET Framework）:**
- System.Runtime, System.Text.Json, System.Console, etc.
- 全て v9.0.0.0
- `/usr/local/share/dotnet/shared/Microsoft.NETCore.App/9.0.0/` から読み込み

**重要な観察:**
- カスタムアセンブリはローカルのbinディレクトリから読み込み
- システムアセンブリは共有ランタイムから読み込み
- 必要なアセンブリのみが動的に読み込まれている

### 3. deps.jsonファイル構造分析（開発環境）

```json
{
  "runtimeTarget": {
    "name": ".NETCoreApp,Version=v9.0",
    "signature": ""
  },
  "compilationOptions": {},
  "targets": {
    ".NETCoreApp,Version=v9.0": {
      "DepsJsonDemo/1.0.0": {
        "dependencies": {
          "CommonFramework": "1.0.0"  // プロジェクト間依存関係
        },
        "runtime": {
          "DepsJsonDemo.dll": {}
        }
      },
      "CommonFramework/1.0.0": {
        "runtime": {
          "CommonFramework.dll": {
            "assemblyVersion": "1.0.0",
            "fileVersion": "1.0.0.0"
          }
        }
      }
    }
  },
  "libraries": {
    "DepsJsonDemo/1.0.0": {
      "type": "project",           // プロジェクト参照
      "serviceable": false,
      "sha512": ""
    },
    "CommonFramework/1.0.0": {
      "type": "project",           // プロジェクト参照
      "serviceable": false,
      "sha512": ""
    }
  }
}
```

**構造分析:**

#### `runtimeTarget`
- ターゲットフレームワークを指定: .NETCoreApp,Version=v9.0
- 署名は空（開発ビルドのため）

#### `targets`
- フレームワーク固有の依存関係を定義
- DepsJsonDemo → CommonFramework の依存関係が明確に記録
- 各アセンブリのランタイムファイルとバージョン情報

#### `libraries`
- 全ライブラリのメタデータ
- `type: "project"` = プロジェクト参照（NuGetパッケージではない）
- `serviceable: false` = サービス可能でない（ローカルプロジェクト）
- `sha512: ""` = ハッシュ値なし（開発ビルド）

### 4. Publish版との比較分析

#### Publishビルド実行
```bash
$ dotnet publish src/DepsJsonDemo/DepsJsonDemo.csproj -o ./publish-result --configuration Release
```

**結果:**
- ✅ **成功**: Release構成でpublish完了
- **ビルド時間**: 4.5秒（Releaseビルドのため若干長い）
- **出力先**: `./publish-result/`

#### 実行結果の比較

**共通点:**
- 基本的な動作は同じ
- アセンブリ情報とdeps.json構造は同一
- 依存関係の解決メカニズムも同一

**相違点:**
- **ファイル配置**: publish-resultディレクトリに独立配置
- **実行ファイル**: `./publish-result/DepsJsonDemo` として直接実行可能
- **依存関係**: 必要なファイルがすべて同一ディレクトリに配置

### 5. DependencyAnalyzer詳細解析結果

```
=== deps.json解析結果: DepsJsonDemo.deps.json ===
--- 基本情報 ---
ランタイムターゲット: .NETCoreApp,Version=v9.0
署名: (空)
コンパイルオプション: (なし)

--- ライブラリ情報 ---
総ライブラリ数: 2
NuGetパッケージ: 0
プロジェクト参照: 2

--- ランタイム依存関係 ---
DepsJsonDemo/1.0.0:
  → CommonFramework: 1.0.0

--- 統計情報 ---
ランタイムファイル数: 2
```

**重要な発見:**

1. **依存関係の種類**
   - NuGetパッケージ: 0個
   - プロジェクト参照: 2個
   - これは純粋なプロジェクト間参照のみの構成

2. **依存関係グラフ**
   - DepsJsonDemo が CommonFramework に依存
   - シンプルな1対1の依存関係

3. **ランタイムファイル**
   - 実際に配布が必要なアセンブリは2個のみ
   - システムアセンブリは.NETランタイムから供給

## 📊 学習成果と洞察

### deps.jsonの役割の実証

1. **依存関係管理**
   - ✅ プロジェクト間の依存関係が正確に記録
   - ✅ バージョン情報の保持
   - ✅ ランタイムでの正確な解決

2. **パフォーマンス最適化**
   - ✅ 必要なアセンブリの事前特定
   - ✅ 動的ロードの最小化
   - ✅ ファイルシステム探索の削減

3. **配布時の信頼性**
   - ✅ 必要ファイルの明確な特定
   - ✅ バージョン整合性の確保
   - ✅ 実行時エラーの予防

### NuGetパッケージとの比較考察

現在のプロジェクトは **プロジェクト参照のみ** を使用しており、実際のNuGetパッケージは含まれていません。今後の学習として、以下を追加することを推奨：

1. **外部NuGetパッケージの追加**
   - `dotnet add package Newtonsoft.Json`
   - deps.jsonでの違いを観察

2. **CommonFrameworkのNuGetパッケージ化**
   - `dotnet pack`でパッケージ作成
   - GitHub Packagesでの配布

3. **複雑な依存関係グラフの作成**
   - 推移的依存関係の確認
   - バージョン競合の解決メカニズム

### 次のステップ

1. **追加NuGetパッケージでの実験**
2. **Self-contained deployment での動作確認**
3. **GitHub Actions での自動ビルド・テスト**
4. **実際のNuGetパッケージ作成と配布**

---

このログは、deps.jsonの基本的な仕組みと効果を実際のコードで確認した貴重な記録です。今後の.NET開発でのより深い理解の基礎となります。
