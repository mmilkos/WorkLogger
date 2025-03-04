using Microsoft.AspNetCore.Mvc;

namespace WorkLogger.Controllers;

public class FallBackController : Controller
{
    public ActionResult Index()
    {
        return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/browser", "index.html"), "text/HTML");
    }
}