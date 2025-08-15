#!/bin/bash

# 🚀 GitHub Packages自動リリーススクリプト
# 使用方法: ./scripts/create-release.sh 1.0.0 "初回リリース"

set -e

# 引数チェック
if [ $# -lt 1 ]; then
    echo "❌ 使用方法: $0 <version> [release_description]"
    echo "例: $0 1.0.0 \"初回リリース\""
    exit 1
fi

VERSION=$1
DESCRIPTION=${2:-"Release v$VERSION"}

echo "🚀 GitHub Packages自動リリース開始"
echo "バージョン: v$VERSION"
echo "説明: $DESCRIPTION"
echo ""

# 現在のブランチ確認
CURRENT_BRANCH=$(git branch --show-current)
if [ "$CURRENT_BRANCH" != "main" ]; then
    echo "⚠️  警告: 現在のブランチは '$CURRENT_BRANCH' です"
    read -p "mainブランチに切り替えますか？ (y/N): " -n 1 -r
    echo
    if [[ $REPLY =~ ^[Yy]$ ]]; then
        git checkout main
        git pull origin main
    else
        echo "❌ リリースはmainブランチから作成することを推奨します"
        exit 1
    fi
fi

# 未コミットの変更確認
if ! git diff-index --quiet HEAD --; then
    echo "❌ 未コミットの変更があります。先にコミットしてください。"
    git status
    exit 1
fi

# タグが既に存在するかチェック
if git tag -l | grep -q "^v$VERSION$"; then
    echo "❌ タグ v$VERSION は既に存在します"
    exit 1
fi

# 最終確認
echo "📋 リリース内容の確認:"
echo "- バージョン: v$VERSION"
echo "- ターゲットブランチ: $CURRENT_BRANCH"
echo "- 最新コミット: $(git log --oneline -1)"
echo ""

read -p "このリリースを作成しますか？ (y/N): " -n 1 -r
echo
if [[ ! $REPLY =~ ^[Yy]$ ]]; then
    echo "❌ リリース作成をキャンセルしました"
    exit 1
fi

# タグ作成とプッシュ
echo "🏷️  タグ v$VERSION を作成中..."
git tag -a "v$VERSION" -m "$DESCRIPTION"

echo "📤 タグをGitHubにプッシュ中..."
git push origin "v$VERSION"

echo ""
echo "✅ リリース v$VERSION の作成が開始されました！"
echo ""
echo "📊 進行状況の確認:"
echo "1. GitHub Actions: https://github.com/$GITHUB_REPOSITORY/actions"
echo "2. リリースページ: https://github.com/$GITHUB_REPOSITORY/releases"
echo "3. パッケージ: https://github.com/$GITHUB_REPOSITORY/packages"
echo ""
echo "🎉 約5-10分でパッケージが利用可能になります"
