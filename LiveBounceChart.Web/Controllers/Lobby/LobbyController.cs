using System.Web.Mvc;
using LiveBounceChart.Web.DAL;

namespace LiveBounceChart.Web.Controllers.Lobby
{
    public class LobbyController : Controller
    {
        private readonly IBounceDB _context;

        public LobbyController()
        {
        }

        public LobbyController(IBounceDB context)
        {
            _context = context;
        }

        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

    }
}
