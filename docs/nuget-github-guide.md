# NuGetパッケージ管理とGitHub活用ガイド

## 概要

このガイドでは、.NETプロジェクトで共通フレームワークをNuGetパッケージとして管理し、GitHubで効率的に運用する方法を説明します。

## 1. NuGetパッケージ作成の基本

### プロジェクト設定

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <GeneratePackageOnBuild>true</GeneratePackageOnBuild>
    <PackageId>MyCompany.CommonFramework</PackageId>
    <PackageVersion>1.0.0</PackageVersion>
    <Authors>開発チーム</Authors>
    <Company>MyCompany</Company>
    <Description>社内共通フレームワーク</Description>
    <PackageTags>framework;common;internal</PackageTags>
    <RepositoryUrl>https://github.com/mycompany/common-framework</RepositoryUrl>
    <RepositoryType>git</RepositoryType>
    <PackageLicenseExpression>MIT</PackageLicenseExpression>
  </PropertyGroup>
</Project>
```

### パッケージ作成コマンド

```bash
# パッケージのビルド
dotnet pack --configuration Release

# 特定のバージョンでパッケージ作成
dotnet pack -p:PackageVersion=1.2.3 --configuration Release
```

## 2. GitHub Packages の活用

### GitHub Packagesとは

- GitHubが提供するパッケージレジストリ
- プライベートパッケージの管理に適している
- GitHubアカウントと統合された認証

### 設定手順

#### 1. Personal Access Token (PAT) の作成

1. GitHub > Settings > Developer settings > Personal access tokens
2. "Generate new token" をクリック
3. 必要なスコープを選択:
   - `read:packages`
   - `write:packages`
   - `delete:packages`

#### 2. NuGet設定ファイルの作成

```xml
<!-- nuget.config -->
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <add key="github" value="https://nuget.pkg.github.com/ORGANIZATION/index.json" />
  </packageSources>
  <packageSourceCredentials>
    <github>
      <add key="Username" value="YOUR_GITHUB_USERNAME" />
      <add key="ClearTextPassword" value="YOUR_PERSONAL_ACCESS_TOKEN" />
    </github>
  </packageSourceCredentials>
</configuration>
```

#### 3. パッケージの公開

```bash
# GitHub Packagesに公開
dotnet nuget push "bin/Release/MyCompany.CommonFramework.1.0.0.nupkg" \
  --api-key YOUR_PERSONAL_ACCESS_TOKEN \
  --source "https://nuget.pkg.github.com/ORGANIZATION/index.json"
```

## 3. プライベートNuGetサーバーの構築

### Azure Artifacts の利用

```bash
# Azure DevOps CLIのインストール（必要に応じて）
az extension add --name azure-devops

# パッケージの公開
dotnet nuget push "package.nupkg" \
  --api-key azuredevops \
  --source "https://pkgs.dev.azure.com/ORGANIZATION/PROJECT/_packaging/FEED/nuget/v3/index.json"
```

### 自前のNuGetサーバー

```bash
# BaGetサーバーのセットアップ（Docker使用）
docker run -d \
  -p 5000:80 \
  -e "ApiKey=YOUR_API_KEY" \
  -e "Storage__Type=FileSystem" \
  -e "Storage__Path=/var/baget/packages" \
  -v "$(pwd)/baget-data:/var/baget" \
  loicsharma/baget:latest
```

## 4. バージョン管理戦略

### セマンティックバージョニング

```
MAJOR.MINOR.PATCH

MAJOR: 破壊的変更
MINOR: 新機能追加（後方互換性あり）
PATCH: バグ修正
```

### プレリリース版

```xml
<PackageVersion>1.0.0-alpha.1</PackageVersion>
<PackageVersion>1.0.0-beta.2</PackageVersion>
<PackageVersion>1.0.0-rc.1</PackageVersion>
```

## 5. GitHub Actions による自動化

### パッケージ自動公開ワークフロー

```yaml
# .github/workflows/publish-package.yml
name: Publish NuGet Package

on:
  push:
    tags:
      - 'v*'

jobs:
  publish:
    runs-on: ubuntu-latest
    steps:
    - uses: actions/checkout@v3
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: '8.0.x'
    
    - name: Restore dependencies
      run: dotnet restore
    
    - name: Build
      run: dotnet build --configuration Release --no-restore
    
    - name: Test
      run: dotnet test --configuration Release --no-build
    
    - name: Pack
      run: dotnet pack --configuration Release --no-build --output ./nupkg/
    
    - name: Publish to GitHub Packages
      run: |
        dotnet nuget push ./nupkg/*.nupkg \
          --api-key ${{ secrets.GITHUB_TOKEN }} \
          --source "https://nuget.pkg.github.com/${{ github.repository_owner }}/index.json"
```

## 6. 依存関係管理のベストプラクティス

### Central Package Management

```xml
<!-- Directory.Packages.props -->
<Project>
  <PropertyGroup>
    <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
  </PropertyGroup>
  
  <ItemGroup>
    <PackageVersion Include="System.Text.Json" Version="8.0.0" />
    <PackageVersion Include="Microsoft.Extensions.DependencyInjection" Version="8.0.0" />
  </ItemGroup>
</Project>
```

### フロート版号の回避

```xml
<!-- 避けるべき -->
<PackageReference Include="MyPackage" Version="1.*" />

<!-- 推奨 -->
<PackageReference Include="MyPackage" Version="1.2.3" />
```

## 7. セキュリティ考慮事項

### 脆弱性スキャン

```bash
# 脆弱性のあるパッケージをチェック
dotnet list package --vulnerable

# 古いパッケージをチェック
dotnet list package --outdated
```

### パッケージ署名

```bash
# パッケージに署名
dotnet nuget sign package.nupkg \
  --certificate-path cert.p12 \
  --certificate-password password
```

## 8. 監視とメトリクス

### パッケージ使用状況の追跡

- ダウンロード数の監視
- 依存関係グラフの分析
- バージョン採用率の確認

### 自動更新戦略

```bash
# Dependabotの設定例（.github/dependabot.yml）
version: 2
updates:
  - package-ecosystem: "nuget"
    directory: "/"
    schedule:
      interval: "weekly"
    reviewers:
      - "team-lead"
```

## まとめ

効果的なNuGetパッケージ管理には以下が重要です：

1. **明確なバージョニング戦略**
2. **自動化されたCI/CDパイプライン**
3. **適切なセキュリティ対策**
4. **継続的な監視と更新**

これらを実践することで、保守性が高く、安全なソフトウェア開発が実現できます。
