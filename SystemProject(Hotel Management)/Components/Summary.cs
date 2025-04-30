using Microsoft.AspNetCore.Mvc;

namespace SystemProject_Hotel_Management_.Component
{
    public class Summary:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
