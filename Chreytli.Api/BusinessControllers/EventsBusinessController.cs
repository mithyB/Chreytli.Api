using Chreytli.Api.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Chreytli.Api.BusinessControllers
{
    public class EventsBusinessController
    {
        public IQueryable<Event> GetEvents(IEnumerable<Event> events, IDbSet<ApplicationUser> users)
        {
            return events.AsQueryable(); /*.Select(x =>
            {
                if (x.Author.Id != null)
                {
                    var author = users.Find(x.Author.Id);
                    x.Author = new
                    {
                        author.UserName,
                        author.Id
                    };
                }
                else
                {
                    x.Author = new { UserName = "unknown" };
                }

                return x;
            }).AsQueryable(); */
        }
    }
}