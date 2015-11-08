using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chreytli.Api.Controllers
{
    public class RiotApiController : Controller
    {
        // GET: RiotApi
        public ActionResult Index()
        {
            return View();
        }

        public string GetMatchHistory()
        {
            return "Kappa";
        }
    }
}