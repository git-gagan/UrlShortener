using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UrlShortener.Data;
using UrlShortener.Models;

namespace UrlShortener.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public new IActionResult Url()
        {
            return View();
        }

        [HttpPost]
        public new IActionResult UrlPost(UrlsViewModel obj)
        {
            ViewData["originalUrl"] = obj.longUrl;
            var urlList = _db.Urls;
            Console.WriteLine(urlList);
            bool url_already_exists = false;
            string mapped_short_url = null;
            foreach (var url in urlList)
            {
                if (obj.longUrl == url.longUrl)
                {
                    url_already_exists = true;
                    mapped_short_url = url.shortUrl;
                    break;
                }
                Console.WriteLine(url.longUrl);
            }
            if (url_already_exists)
            {
                Console.WriteLine("URL already Exists");
                Console.WriteLine(mapped_short_url);
                ViewData["shortUrl"] = mapped_short_url;
                string[] url_arr = mapped_short_url.Split("/");
                ViewData["urlSuffix"] = url_arr[url_arr.Length - 1];
                Console.WriteLine(ViewData["urlSuffix"]);
                return View();
            }
            else
            {
                Random rnd = new Random();
                int suffix = rnd.Next();
                string host = HttpContext.Request.Host.ToString();
                string shortUrl = host + $"/{suffix}";
                Console.WriteLine($"Short URL:- {shortUrl}");
                ViewData["shortUrl"] = shortUrl;
                ViewData["urlSuffix"] = suffix;
                Console.WriteLine(obj);
                Console.WriteLine(obj.longUrl);
                var new_obj = new UrlsViewModel
                {
                    longUrl = obj.longUrl,
                    shortUrl = shortUrl,
                };
                _db.Urls.Add(new_obj);
                _db.SaveChanges();
                Console.WriteLine("Saved to DB");
                return View();
            }
        }
            

        public new IActionResult MyUrl(int Id)
        {
            string currentShortUrl = HttpContext.Request.Host.ToString() + $"/{Id}";
            var obj = _db.Urls.Where(o => o.shortUrl == currentShortUrl).Single();
            string originalUrl = obj.longUrl;
            Console.WriteLine($"============= {originalUrl}");
            return Redirect(originalUrl);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}