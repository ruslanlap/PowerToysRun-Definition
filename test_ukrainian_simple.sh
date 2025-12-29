#!/bin/bash

echo "=== Тест української транслітерації ==="
echo ""

# Test transliteration manually
test_word() {
    local cyrillic=$1
    local expected_latin=$2

    echo "Тестую: $cyrillic"
    echo "Очікується: $expected_latin"

    # Make actual request
    local url="https://sum.in.ua/s/$expected_latin"
    echo "URL: $url"

    local response=$(curl -s "$url")

    if echo "$response" | grep -q "не знайдено"; then
        echo "❌ СЛОВО НЕ ЗНАЙДЕНО (404)"
    elif echo "$response" | grep -q "itemprop=\"articleBody\""; then
        echo "✅ ЗНАЙДЕНО ВИЗНАЧЕННЯ!"
        echo "Перші 200 символів:"
        echo "$response" | grep -oP '(?<=<div itemprop="articleBody">).*?(?=</div>)' | head -c 200
        echo "..."
    else
        echo "⚠️  Неочікувана відповідь"
    fi
    echo ""
    echo "---"
    echo ""
}

echo "Тест 1: слово → slovo"
test_word "слово" "slovo"

echo "Тест 2: мова → mova"
test_word "мова" "mova"

echo "Тест 3: неіснуюче слово"
local url="https://sum.in.ua/s/xyzqweasdzxc123"
echo "URL: $url"
local response=$(curl -s "$url")
if echo "$response" | grep -q "не знайдено"; then
    echo "✅ ПРАВИЛЬНО визначено як 'не знайдено'"
else
    echo "❌ Має бути 'не знайдено'"
fi

echo ""
echo "=== Тест завершено ==="
