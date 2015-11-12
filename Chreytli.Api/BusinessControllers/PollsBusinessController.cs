

using Chreytli.Api.Models;
using System.Data.Entity;
using System.Linq;

namespace Chreytli.Api.BusinessControllers
{
    public class PollsBusinessController
    {
        public IQueryable<Poll> GetPolls(IDbSet<Poll> polls, IDbSet<Vote> votes, IDbSet<ApplicationUser> users, string userId, int pageSize, int page)
        {
            return polls.Include(x => x.Choices)
                .OrderByDescending(x => x.Date)
                .Skip(pageSize * page).Take(pageSize)
                .ToList()
                .Select(x =>
            {
                x.IsVoted = votes.Any(y => y.User.Id == userId && y.Poll.Id == x.Id);
                x.Choices = x.Choices.OrderByDescending(y => y.Votes).ToList();
                return x;
            }).AsQueryable();
        }
    }
}