using JsonBasedLocalization.web.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Diagnostics;

namespace JsonBasedLocalization.web.Controllers;
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IStringLocalizer<HomeController> _stringLocalizer;

    public HomeController(ILogger<HomeController> logger, IStringLocalizer<HomeController> stringLocalizer) 
    {
        _logger = logger;
        _stringLocalizer = stringLocalizer;
    }

    public IActionResult Index() 
    {
        ViewBag.Welcome = string.Format(_stringLocalizer["welcome"], "Muhammad Jamal");
        return View();
    }

    public IActionResult Privacy() 
    {
        return View();
    }

    [HttpPost]
    public IActionResult SetLanguage(string culture, string redirecturl) 
    {
        Response.Cookies.Append
            (
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddDays(7) }
            );
        return LocalRedirect(redirecturl);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() 
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
