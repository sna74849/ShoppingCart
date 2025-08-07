using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models;
using System.Diagnostics;
using System.Text.Json;

namespace ShoppingCart.Controllers
{
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger) {
            _logger = logger;
        }
        public IActionResult Index() {
            return View();
        }
        public IActionResult Privacy() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
    
    /// <summary>
    /// セッション拡張クラス
    /// </summary>
    public static class SessionExtensions {
        // セッションにオブジェクトを書き込む
        public static void SetObject<T>(this ISession session, string key, T obj) {
            var json = JsonSerializer.Serialize(obj);
            session.SetString(key, json);
        }
        // セッションからオブジェクトを読み込む
        public static T? GetObject<T>(this ISession session, string key) {
            var json = session.GetString(key);
            return string.IsNullOrEmpty(json)
            ? default
            : JsonSerializer.Deserialize<T>(json);
        }
    }
}