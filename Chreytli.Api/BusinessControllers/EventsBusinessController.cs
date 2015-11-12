using Chreytli.Api.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Chreytli.Api.BusinessControllers
{
    public class EventsBusinessController
    {
        public IQueryable<Event> GetEvents(IDbSet<Event> events, IDbSet<ApplicationUser> users)
        {
            return events.Include(x => x.Author).ToList().AsQueryable();
        }
    }
}