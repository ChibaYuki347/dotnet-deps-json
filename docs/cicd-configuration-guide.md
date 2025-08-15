# 🔧 CI/CD環境での設定管理

## 📋 概要

このプロジェクトでは、ローカル開発環境とCI/CD環境で異なるNuGet設定を使用します：

- **ローカル開発**: `local-nuget-feed`を使用した実験的パッケージテスト
- **CI/CD環境**: GitHub Packagesを使用した本番パッケージ配布

## 🏗️ CI/CDワークフローでの自動設定

### 設定の自動生成

GitHub Actionsワークフローでは、実行時に適切な`nuget.config`を動的に生成します：

```yaml
- name: Configure NuGet for CI/CD
  run: |
    # CI/CD用のnuget.configを作成（ローカルフィード除外）
    cat > nuget.config << 'EOF'
    <?xml version="1.0" encoding="utf-8"?>
    <configuration>
      <packageSources>
        <add key="nuget.org" value="https://api.nuget.org/v3/index.json" protocolVersion="3" />
        <add key="github-packages" value="https://nuget.pkg.github.com/${{ github.repository_owner }}/index.json" protocolVersion="3" />
      </packageSources>
      <packageSourceCredentials>
        <github-packages>
          <add key="Username" value="${{ github.actor }}" />
          <add key="ClearTextPassword" value="${{ secrets.GITHUB_TOKEN }}" />
        </github-packages>
      </packageSourceCredentials>
    </configuration>
    EOF
```

### ワークフロー別設定

#### 1. Build Workflow (build.yml)
- **目的**: プルリクエストとmainブランチでのビルド検証
- **設定**: nuget.orgのみ（外部依存なし）
- **対象**: プロジェクト参照ベースの開発

#### 2. Publish Workflow (publish.yml)  
- **目的**: タグベースでのパッケージ公開
- **設定**: nuget.orgのみ（公開専用）
- **対象**: CommonFrameworkの配布

#### 3. Release Workflow (release.yml)
- **目的**: 完全な自動リリースとテスト
- **設定**: nuget.org + GitHub Packages（認証付き）
- **対象**: パッケージ公開と利用テスト

## 🔐 認証設定

### GitHub Packages認証

CI/CD環境では`GITHUB_TOKEN`を使用した自動認証：

```yaml
packageSourceCredentials:
  github-packages:
    add key="Username" value="${{ github.actor }}"
    add key="ClearTextPassword" value="${{ secrets.GITHUB_TOKEN }}"
```

### 権限設定

ワークフローファイルで必要な権限を明示的に設定：

```yaml
permissions:
  contents: write    # リリース作成用
  packages: write    # パッケージ公開用
```

## 🚨 トラブルシューティング

### よくあるエラーと解決策

#### 1. NU1301: The local source doesn't exist

**エラー例**:
```text
error NU1301: The local source '/home/runner/work/.../local-nuget-feed' doesn't exist.
```

**原因**: CI/CD環境でローカルフィード設定が残っている

**解決策**: ワークフローで動的にnuget.configを再生成

#### 2. NETSDK1045: .NET SDK version mismatch

**エラー例**:
```text
error NETSDK1045: The current .NET SDK does not support targeting .NET 9.0
```

**原因**: GitHub Actionsで古いSDKバージョンを使用

**解決策**: `dotnet-version: '9.0.x'`に更新

#### 3. 認証エラー

**エラー例**:
```text
error NU1301: Unable to load the service index for source
```

**原因**: GitHub Packages認証設定の不備

**解決策**: packageSourceCredentialsの正しい設定

## 📈 設定の検証

### ローカルでの確認

```bash
# 現在の設定確認
dotnet nuget list source

# ローカルフィード使用時
dotnet restore

# CI/CD設定テスト
rm nuget.config
dotnet restore --source https://api.nuget.org/v3/index.json
```

### CI/CDでの確認

GitHub Actionsログで以下を確認：

1. **NuGet設定生成**: `Configure NuGet for CI/CD`ステップ
2. **パッケージソース**: `dotnet restore`の出力
3. **認証状況**: パッケージダウンロードの成功/失敗

## 🔄 設定の同期

### ローカル開発からCI/CDへの移行

1. **プロジェクト参照からパッケージ参照へ**:
   ```xml
   <!-- Before: プロジェクト参照 -->
   <ProjectReference Include="../CommonFramework/CommonFramework.csproj" />
   
   <!-- After: パッケージ参照 -->
   <PackageReference Include="CommonFramework" Version="1.0.0" />
   ```

2. **ターゲットフレームワーク統一**:
   ```xml
   <!-- 全プロジェクトでnet9.0に統一 -->
   <TargetFramework>net9.0</TargetFramework>
   ```

3. **依存関係の更新**:
   ```bash
   # パッケージ参照更新
   dotnet add package CommonFramework --source "https://nuget.pkg.github.com/ChibaYuki347/index.json"
   ```

## 🎯 最適化のポイント

1. **キャッシュ活用**: NuGetパッケージのキャッシュでビルド時間短縮
2. **条件分岐**: 環境に応じた設定の動的切り替え
3. **エラーハンドリング**: 設定エラーの詳細ログ出力
4. **セキュリティ**: 認証情報の適切な管理

これらの設定により、ローカル開発の柔軟性を保ちながら、CI/CD環境での堅牢な自動化を実現しています。
