#!/bin/bash

set -e  # Зупинити скрипт при першій помилці

echo "🧹 Очищення попередніх артефактів..."
rm -rf ./Definition/Publish
rm -rf ./Definition/Community.PowerToys.Run.Plugin.Definition/obj
rm -rf ./Definition-x64.zip
rm -rf ./Definition-ARM64.zip
rm -rf ./Definition/Community.PowerToys.Run.Plugin.Definition/bin

PROJECT_PATH="Definition/Community.PowerToys.Run.Plugin.Definition/Community.PowerToys.Run.Plugin.Definition.csproj"
OUT_ROOT="./Definition/Community.PowerToys.Run.Plugin.Definition/bin"
PLUGIN_NAME="Definition"

# 1. Побудова для x64
echo "🛠️  Building for x64..."
dotnet publish "$PROJECT_PATH" -c Release -r win-x64 -p:Platform=x64 -p:PlatformTarget=x64

# 2. Побудова для ARM64
echo "🛠️  Building for ARM64..."
dotnet publish "$PROJECT_PATH" -c Release -r win-arm64 -p:Platform=ARM64 -p:PlatformTarget=ARM64

# 3. Створення zip для x64 (готовий для розпакування в папку плагінів)
echo "📦 Creating x64 zip..."
PUBLISH_X64="$OUT_ROOT/x64/Release/net9.0-windows10.0.22621.0/win-x64/publish"

# Створюємо тимчасову папку з ім'ям плагіна для правильної структури
TEMP_DIR=$(mktemp -d)
mkdir -p "$TEMP_DIR/$PLUGIN_NAME"
cp -r "$PUBLISH_X64"/* "$TEMP_DIR/$PLUGIN_NAME/"

# Архівуємо з правильною структурою (папка Definition/ всередині)
ZIP_X64="./Definition-x64.zip"
(cd "$TEMP_DIR" && zip -r - "$PLUGIN_NAME") > "$ZIP_X64"
rm -rf "$TEMP_DIR"

# 4. Створення zip для ARM64
echo "📦 Creating ARM64 zip..."
PUBLISH_ARM64="$OUT_ROOT/ARM64/Release/net9.0-windows10.0.22621.0/win-arm64/publish"

TEMP_DIR=$(mktemp -d)
mkdir -p "$TEMP_DIR/$PLUGIN_NAME"
cp -r "$PUBLISH_ARM64"/* "$TEMP_DIR/$PLUGIN_NAME/"

ZIP_ARM64="./Definition-ARM64.zip"
(cd "$TEMP_DIR" && zip -r - "$PLUGIN_NAME") > "$ZIP_ARM64"
rm -rf "$TEMP_DIR"

# 5. Також копіюємо в Publish для зручності
echo "📂 Copying to Publish folder..."
DEST_DIR="./Definition/Publish"
rm -rf "$DEST_DIR"
mkdir -p "$DEST_DIR"
cp -r "$PUBLISH_X64"/* "$DEST_DIR/"

echo ""
echo "✅ Готово! Створено:"
echo "   📦 $ZIP_X64 ($(du -h "$ZIP_X64" | cut -f1))"
echo "   📦 $ZIP_ARM64 ($(du -h "$ZIP_ARM64" | cut -f1))"
echo "   📂 $DEST_DIR/"
echo ""
echo "📋 Інструкція:"
echo "   1. Розпакуйте Definition-x64.zip в:"
echo "      %LOCALAPPDATA%\\Microsoft\\PowerToys\\PowerToys Run\\Plugins\\"
echo "   2. Перезапустіть PowerToys"
echo "   3. Тестуйте: def слово, def мова, def тин"
