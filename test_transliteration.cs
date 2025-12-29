using System;
using System.Collections.Generic;
using System.Text;

class TransliterationTest
{
    static void Main()
    {
        var testWords = new[] {
            "слово",
            "привіт",
            "дякую",
            "українська",
            "щастя",
            "їжак",
            "є"
        };

        foreach (var word in testWords)
        {
            var transliterated = TransliterateCyrillicToLatin(word);
            var url = $"https://sum.in.ua/s/{transliterated}";
            Console.WriteLine($"{word} -> {transliterated} -> {url}");
        }
    }

    static string TransliterateCyrillicToLatin(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        var transliteration = new Dictionary<char, string>
        {
            {'а', "a"}, {'б', "b"}, {'в', "v"}, {'г', "g"}, {'ґ', "g"}, {'д', "d"},
            {'е', "e"}, {'є', "je"}, {'ж', "zh"}, {'з', "z"}, {'и', "y"}, {'і', "i"},
            {'ї', "ji"}, {'й', "j"}, {'к', "k"}, {'л', "l"}, {'м', "m"}, {'н', "n"},
            {'о', "o"}, {'п', "p"}, {'р', "r"}, {'с', "s"}, {'т', "t"}, {'у', "u"},
            {'ф', "f"}, {'х', "h"}, {'ц', "c"}, {'ч', "ch"}, {'ш', "sh"}, {'щ', "shh"},
            {'ь', "j"}, {'ю', "ju"}, {'я', "ja"},
            {'А', "A"}, {'Б', "B"}, {'В', "V"}, {'Г', "G"}, {'Ґ', "G"}, {'Д', "D"},
            {'Е', "E"}, {'Є', "Je"}, {'Ж', "Zh"}, {'З', "Z"}, {'И', "Y"}, {'І', "I"},
            {'Ї', "Ji"}, {'Й', "J"}, {'К', "K"}, {'Л', "L"}, {'М', "M"}, {'Н', "N"},
            {'О', "O"}, {'П', "P"}, {'Р', "R"}, {'С', "S"}, {'Т', "T"}, {'У', "U"},
            {'Ф', "F"}, {'Х', "H"}, {'Ц', "C"}, {'Ч', "Ch"}, {'Ш', "Sh"}, {'Щ', "Shh"},
            {'Ь', "J"}, {'Ю', "Ju"}, {'Я', "Ja"}
        };

        var result = new StringBuilder(input.Length * 2);
        foreach (var c in input)
        {
            if (transliteration.TryGetValue(c, out string replacement))
                result.Append(replacement);
            else
                result.Append(c);
        }

        return result.ToString();
    }
}
