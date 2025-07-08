using System;
using System.Collections.Generic;
using Wox.Plugin;

namespace Community.PowerToys.Run.Plugin.Definition
{
    internal class DictionaryEntry
    {
        public string Word { get; set; }
        public string Phonetic { get; set; }
        public List<Phonetic> Phonetics { get; set; } = new();
        public List<Meaning> Meanings { get; set; } = new();
        public LicenseInfo License { get; set; }
        public List<string> SourceUrls { get; set; } = new();
    }

    internal class Phonetic
    {
        public string Text { get; set; }
        public string Audio { get; set; }
        public string SourceUrl { get; set; }
        public LicenseInfo License { get; set; }
    }

    internal class Meaning
    {
        public string PartOfSpeech { get; set; }
        public List<DefinitionItem> Definitions { get; set; } = new();
        public List<string> Synonyms { get; set; } = new();
        public List<string> Antonyms { get; set; } = new();
    }

    internal class DefinitionItem
    {
        public string Definition { get; set; }
        public string Example { get; set; }
    }

    internal class LicenseInfo
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }

    internal class ResultContext
    {
        public string TextToCopy { get; set; }
        public string AudioUrl { get; set; }
        public string SourceUrl { get; set; }
        public string Word { get; set; }
    }

    internal class CacheItem
    {
        public List<Result> Results { get; }
        public DateTime Timestamp { get; }
        public CacheItem Previous { get; set; }
        public CacheItem Next { get; set; }
        public string Key { get; set; }

        public CacheItem(List<Result> results, DateTime timestamp, string key = null)
        {
            Results = results;
            Timestamp = timestamp;
            Key = key;
        }

        public bool IsExpired => DateTime.UtcNow - Timestamp > TimeSpan.FromMinutes(ConfigurationManager.Configuration.CacheExpirationMinutes);
    }

    internal class LRUCache
    {
        private readonly int _capacity;
        private readonly Dictionary<string, CacheItem> _cache;
        private readonly CacheItem _head;
        private readonly CacheItem _tail;

        public LRUCache(int capacity)
        {
            _capacity = capacity;
            _cache = new Dictionary<string, CacheItem>(capacity);
            _head = new CacheItem(null, DateTime.MinValue);
            _tail = new CacheItem(null, DateTime.MinValue);
            _head.Next = _tail;
            _tail.Previous = _head;
        }

        public bool TryGetValue(string key, out CacheItem item)
        {
            if (_cache.TryGetValue(key, out item))
            {
                if (item.IsExpired)
                {
                    RemoveItem(key);
                    item = null;
                    return false;
                }
                
                // Move to head (most recently used)
                RemoveFromList(item);
                AddToHead(item);
                return true;
            }
            
            return false;
        }

        public void Set(string key, CacheItem item)
        {
            if (_cache.TryGetValue(key, out var existingItem))
            {
                // Update existing item
                RemoveFromList(existingItem);
                _cache[key] = item;
                item.Key = key;
                AddToHead(item);
            }
            else
            {
                // Add new item
                if (_cache.Count >= _capacity)
                {
                    // Remove least recently used item
                    var lru = _tail.Previous;
                    RemoveItem(lru.Key);
                }
                
                _cache[key] = item;
                item.Key = key;
                AddToHead(item);
            }
        }

        public int Count => _cache.Count;

        private void AddToHead(CacheItem item)
        {
            item.Previous = _head;
            item.Next = _head.Next;
            _head.Next.Previous = item;
            _head.Next = item;
        }

        private void RemoveFromList(CacheItem item)
        {
            item.Previous.Next = item.Next;
            item.Next.Previous = item.Previous;
        }

        private void RemoveItem(string key)
        {
            if (_cache.TryGetValue(key, out var item))
            {
                RemoveFromList(item);
                _cache.Remove(key);
            }
        }
    }

    internal static class ResultExtensions
    {
        public static Result Clone(this Result result, string newQueryTextDisplay)
        {
            return new Result
            {
                QueryTextDisplay = newQueryTextDisplay,
                IcoPath = result.IcoPath,
                Title = result.Title,
                SubTitle = result.SubTitle,
                ToolTipData = result.ToolTipData,
                Action = result.Action,
                ContextData = result.ContextData,
                Score = result.Score
            };
        }
    }
} 