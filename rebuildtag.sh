#!/bin/bash
set -euo pipefail

# Script to rebuild a specific tag and trigger a new release
# This script will:
# 1. Delete the existing tag locally and remotely
# 2. Create a new tag at the current commit
# 3. Push the new tag to trigger the GitHub Actions workflow

# Default values
TAG_NAME="v1.0.1"
REMOTE_NAME="origin"
FORCE=false

# Parse command line arguments
while [[ $# -gt 0 ]]; do
  case $1 in
    -t|--tag)
      TAG_NAME="$2"
      shift 2
      ;;
    -r|--remote)
      REMOTE_NAME="$2"
      shift 2
      ;;
    -f|--force)
      FORCE=true
      shift
      ;;
    -h|--help)
      echo "Usage: $0 [TAG_NAME] [-t|--tag TAG_NAME] [-r|--remote REMOTE_NAME] [-f|--force]"
      echo "Options:"
      echo "  TAG_NAME              Tag name as positional argument (e.g., v1.1.2-dev-prev)"
      echo "  -t, --tag TAG_NAME     Specify the tag name (default: v1.0.1)"
      echo "  -r, --remote REMOTE    Specify the remote name (default: origin)"
      echo "  -f, --force           Skip confirmation prompts"
      echo "  -h, --help            Show this help message"
      exit 0
      ;;
    -*)
      echo "Unknown option: $1"
      exit 1
      ;;
    *)
      # Positional argument - treat as tag name if not already set
      if [ "$TAG_NAME" = "v1.0.1" ]; then
        TAG_NAME="$1"
      else
        echo "Error: Multiple tag names specified. Use -t/--tag or positional argument, not both."
        exit 1
      fi
      shift
      ;;
  esac
done

echo "🔄 Rebuilding tag: $TAG_NAME"

# Check if we're in the root of the repository
if [ ! -d ".git" ]; then
    echo "❌ Error: This script must be run from the root of the repository."
    exit 1
fi

# Check if there are uncommitted changes
if ! git diff-index --quiet HEAD --; then
    echo "⚠️ Warning: You have uncommitted changes."
    if [ "$FORCE" = false ]; then
        read -p "Do you want to continue anyway? (y/n) " -n 1 -r
        echo
        if [[ ! $REPLY =~ ^[Yy]$ ]]; then
            echo "Operation cancelled."
            exit 1
        fi
    else
        echo "Proceeding with uncommitted changes due to --force flag."
    fi
fi

# Run operations in parallel where possible

# Delete the local tag
echo "🗑️ Deleting local tag $TAG_NAME..."
git tag -d $TAG_NAME 2>/dev/null || echo "Local tag $TAG_NAME doesn't exist, continuing..."

# Delete the remote tag with --no-verify to skip hooks
echo "🗑️ Deleting remote tag $TAG_NAME..."
git push --no-verify $REMOTE_NAME :refs/tags/$TAG_NAME 2>/dev/null || echo "Remote tag $TAG_NAME doesn't exist or couldn't be deleted, continuing..."

# Create a new tag at the current commit
echo "✨ Creating new tag $TAG_NAME at current commit..."
git tag $TAG_NAME

# Push the new tag to trigger the GitHub Actions workflow
# Use --atomic and --no-verify for faster pushing
echo "📤 Pushing new tag to remote..."
git push --atomic --no-verify $REMOTE_NAME $TAG_NAME

echo "✅ Tag $TAG_NAME has been rebuilt and pushed to $REMOTE_NAME."

echo "🎉 Done!"
echo "🚀 GitHub Actions workflow should start soon at: https://github.com/ruslanlap/PowerToysRun-QuickAi/actions"

# Display estimated completion time
echo "⏱️ Estimated completion time: ~5 minutes"


