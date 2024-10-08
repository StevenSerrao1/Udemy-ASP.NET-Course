using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Social_Media_Links_App_Assignment.Models;

namespace Social_Media_Links_App_Assignment.Controllers
{
    public class HomeController : Controller
    {
        // Create field of Options type to inject
        private readonly SocialMediaLinksOptions _options;

        public HomeController(IOptions<SocialMediaLinksOptions> options)
        {
            // Assign options.Value to use the relevant fields
            _options = options.Value;
        }

        [Route("/")]
        public IActionResult Index()
        {
            ViewBag.Facebook = _options.Facebook;
            ViewBag.Instagram = _options.Instagram;
            ViewBag.Youtube = _options.Youtube;
            ViewBag.Twitter = _options.Twitter;
            return View();
        }
    }
}
