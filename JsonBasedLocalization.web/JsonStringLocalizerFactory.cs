using Microsoft.Extensions.Localization;

namespace JsonBasedLocalization.web;

public class JsonStringLocalizerFactory : IStringLocalizerFactory
{
    public IStringLocalizer Create(Type resourceSource) 
    {
        return new JsonStringLocalizer();
    }

    public IStringLocalizer Create(string baseName, string location) 
    {
        return new JsonStringLocalizer();
    }
}
