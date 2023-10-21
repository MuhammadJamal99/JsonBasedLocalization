using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;

namespace JsonBasedLocalization.web;

public class JsonStringLocalizer : IStringLocalizer
{
    private readonly JsonSerializer _Serializer = new();
    private readonly IDistributedCache _cache;
    public JsonStringLocalizer(IDistributedCache cache) 
    {
        _cache = cache;
    }

    public LocalizedString this[string name]     
    {
        get
        {
            var value = GetString(name);
            return new LocalizedString(name, value);
        }
    }

    public LocalizedString this[string name, params object[] arguments] 
    {
        get
        {
            var actualValue = this[name];
            return !actualValue.ResourceNotFound ? new LocalizedString(name, string.Format(actualValue, arguments)) : actualValue;
        }
    }

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures) 
    {
        string? filePath = $"Resources/{Thread.CurrentThread.CurrentCulture.Name}.json";
        
        using FileStream stream = new(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        using StreamReader streamReader = new(stream);
        using JsonTextReader reader = new(streamReader);

        while (reader.Read())
        {
            if (reader.TokenType != JsonToken.PropertyName)
                continue;

            string? key = reader.Value as string;
            reader.Read();
            string? value = _Serializer.Deserialize<string>(reader);
            yield return new LocalizedString(key, value);

        }

    }

    private string GetString(string key) 
    {
        string? fullFilePath = Path.GetFullPath($"Resources/{Thread.CurrentThread.CurrentCulture.Name}.json");
        //return File.Exists(fullFilePath) ? GetValueFromJson(key, fullFilePath) : string.Empty;

        string cachedKey = $"locale_{Thread.CurrentThread.CurrentCulture.Name}_{key}";
        string? cachedValue = _cache.GetString(cachedKey);

        if(!string.IsNullOrWhiteSpace(cachedValue))
            return cachedValue;

        if (File.Exists(fullFilePath))
        {
            string result = GetValueFromJson(key, fullFilePath);
            _cache.SetString(cachedKey, result);
            return result;
        }
        return string.Empty;
    }

    private string GetValueFromJson(string key, string filePath) 
    {
        if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(filePath))
            return string.Empty;

        using FileStream stream = new(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        using StreamReader streamReader = new(stream);
        using JsonTextReader reader = new(streamReader);

        while (reader.Read())
        {
            if(reader.TokenType == JsonToken.PropertyName && reader.Value as string == key) 
            {
                reader.Read();
                return _Serializer.Deserialize<string>(reader);
            }
        }

        return string.Empty;
    }
}
