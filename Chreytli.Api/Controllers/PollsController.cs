using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Chreytli.Api.Models;
using Chreytli.Api.BusinessControllers;
using Microsoft.AspNet.Identity;

namespace Chreytli.Api.Controllers
{
    public class PollsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private PollsBusinessController controller = new PollsBusinessController();

        // GET: api/Polls
        public IQueryable<Poll> GetPolls([FromUri]string userId = null, [FromUri]int page = 0, [FromUri]int pageSize = 12)
        {
            return controller.GetPolls(db.Polls, db.Votes, db.Users, userId, pageSize, page);
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

            poll.Author = db.Users.Find(User.Identity.GetUserId());
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

            if (poll.Author.Id != User.Identity.GetUserId() &&
                !User.IsInRole("Admins"))
            {
                return Unauthorized();
            }

            await db.Votes.Where(x => x.Poll.Id == poll.Id).ForEachAsync(x =>
            {
                db.VoteChoices.Where(y => y.VoteId == x.Id).ForEachAsync(y => db.VoteChoices.Remove(y));
                db.Votes.Remove(x);
            });
            await db.Choices.Where(x => x.Poll.Id == poll.Id).ForEachAsync(x => db.Choices.Remove(x));

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
                var isVoted = db.Votes.Any(y => y.User.Id == userId && y.Poll.Id == id);

                var voteChoices = new List<VoteChoice>();
                choiceIds.ToList().ForEach(x => voteChoices.Add(new VoteChoice { Choice = db.Choices.Find(x) }));

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

                db.Votes.Add(new Vote { VoteChoices = voteChoices, User = db.Users.Find(userId), Poll = db.Polls.Find(id) });
                db.Entry(poll).Collection(x => x.Choices).Load();

                await db.SaveChangesAsync();

                poll.Choices.ToList().ForEach(x => x.Votes = db.Votes.Count(y => y.VoteChoices.Any(z => z.ChoiceId == x.Id)));
                poll.TotalVotes = poll.Choices.Sum(x => x.Votes);

                await db.SaveChangesAsync();

                return Ok(poll);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.GetBaseException().Message);
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