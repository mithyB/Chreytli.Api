using Chreytli.Api.Models;
using System.Linq;
using System.Web.Http;
using MoreLinq;

namespace Chreytli.Api.Controllers
{
    public class MetaController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        [Route("api/Meta/TopScore")]
        public IHttpActionResult GetTopScore()
        {
            var users = db.Users.Select(x => new { x.Id, x.UserName }).ToArray();

            var result = (from u in users
                          join p in db.Submissions on u.Id equals p.Author.Id into j
                          from s in j.DefaultIfEmpty()
                          select new { u.Id, u.UserName, TotalScore = j.Sum(x => x.Score), PostsCount = j.Count(x => x.Author.Id == u.Id) })
                .DistinctBy(x => x.Id).OrderByDescending(x => x.TotalScore).Where(x => x.TotalScore > 0).Take(10);

            return Ok(result);
        }

        // GET: api/Info/RecentRegistrations
        [HttpGet]
        [Route("api/Meta/RecentRegistrations")]
        public IHttpActionResult GetRecentRegistrations()
        {
            var users = db.Users.Select(x => new { x.Id, x.UserName }).ToArray();

            var result = (from u in users
                          join a in db.Users on u.Id equals a.Id into j
                          from s in j.DefaultIfEmpty()
                          where s != null
                          select new { u.Id, u.UserName, s.CreateDate })
                .OrderByDescending(x => x.CreateDate).Take(10);

            return Ok(result);
        }
    }
}
