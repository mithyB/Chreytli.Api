using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Chreytli.Api.Models;
using MoreLinq;

namespace Chreytli.Api.Controllers
{
    public class PollsController : ApiController
    {
        private ChreytliApiContext db = new ChreytliApiContext();
        private ApplicationDbContext appDb = new ApplicationDbContext();

        // GET: api/Polls
        public IQueryable<Poll> GetPolls([FromUri]string userId = null, [FromUri]int page = 0, [FromUri]int pageSize = 12)
        {
            var polls = db.Polls.Include(x => x.Choices).OrderByDescending(x => x.Date);
            polls.ForEachAsync(x =>
            {
                var user = appDb.Users.Find(x.AuthorId);
                if (user != null)
                {
                    x.Author = new
                    {
                        Username = user.UserName
                    };
                }

                x.IsVoted = db.Votes.Any(y => y.UserId == userId && y.PollId == y.Id);
            });

            return polls.Skip(pageSize * page).Take(pageSize);
        }

        // GET: api/Polls/5
        [ResponseType(typeof(Poll))]
        public async Task<IHttpActionResult> GetPoll(int id)
        {
            Poll poll = await db.Polls.FindAsync(id);
            if (poll == null)
            {
                return NotFound();
            }

            return Ok(poll);
        }

        // PUT: api/Polls/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPoll(int id, Poll poll)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != poll.Id)
            {
                return BadRequest();
            }

            db.Entry(poll).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PollExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Polls
        [ResponseType(typeof(Poll))]
        public async Task<IHttpActionResult> PostPoll(Poll poll)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Polls.Add(poll);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = poll.Id }, poll);
        }

        // DELETE: api/Polls/5
        [ResponseType(typeof(Poll))]
        public async Task<IHttpActionResult> DeletePoll(int id)
        {
            Poll poll = await db.Polls.FindAsync(id);
            if (poll == null)
            {
                return NotFound();
            }

            db.Polls.Remove(poll);
            await db.SaveChangesAsync();

            return Ok(poll);
        }

        [Authorize]
        [HttpPost]
        [Route("api/Polls/{id}/Vote")]
        public async Task<IHttpActionResult> Vote(int id, string userId, int[] choiceIds)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var isVoted = db.Votes.Any(y => y.UserId == userId && y.PollId == id);

                var voteChoices = new List<VoteChoice>();
                choiceIds.ForEach(x => voteChoices.Add(new VoteChoice { ChoiceId = x }));

                //var voteChoices = db.Choices.Where(x => choiceIds.Contains(x.Id)).ToArray();
                var poll = db.Polls.Find(id);

                if (choiceIds.Length > 1 && !poll.MultipleChoice)
                {
                    return BadRequest("This poll is not multiple choice");
                }

                if (poll == null || isVoted || choiceIds.Length == 0)
                {
                    return BadRequest();
                }

                db.Votes.Add(new Vote { VoteChoices = voteChoices, UserId = userId, PollId = id });
                db.Entry(poll).Collection(x => x.Choices).Load();

                await db.SaveChangesAsync();

                poll.Choices.ForEach(x => x.Votes = db.Votes.Count(y => y.VoteChoices.Any(z => z.ChoiceId == x.Id)));
                poll.TotalVotes = poll.Choices.Sum(x => x.Votes);

                await db.SaveChangesAsync();

                return Ok(poll);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PollExists(int id)
        {
            return db.Polls.Count(e => e.Id == id) > 0;
        }
    }
}