using Microsoft.AspNetCore.Mvc;

namespace UP_05.Controllers
{
    public class HomeController : Controller
    {
        public RedirectResult Index() 
        {
            return Redirect("/Registration/Main");
        }
    }
}
