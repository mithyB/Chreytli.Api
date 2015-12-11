using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Chreytli.Api.Models;
using Microsoft.AspNet.Identity;
using Chreytli.Api.BusinessControllers;

namespace Chreytli.Api.Controllers
{
    public class SubmissionsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private SubmissionsBusinessController controller = new SubmissionsBusinessController();

        // GET: api/Submissions
        public IQueryable<Submission> GetSubmissions([FromUri] string userId = null, [FromUri]int page = 0, [FromUri]string[] filter = null)
        {
            const int pageSize = 12;

            return controller.GetSubmissions(db.Submissions, db.Favorites, db.Users, userId, filter, pageSize, page);
        }

        // GET: api/Submissions/5
        [ResponseType(typeof(Submission))]
        public async Task<IHttpActionResult> GetSubmission(int id)
        {
            Submission submission = await db.Submissions.FindAsync(id);
            if (submission == null)
            {
                return NotFound();
            }

            return Ok(submission);
        }

        // PUT: api/Submissions/5
        [Authorize]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSubmission(int id, Submission submission)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != submission.Id)
            {
                return BadRequest();
            }

            db.Entry(submission).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubmissionExists(id))
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

        // POST: api/Submissions
        [Authorize]
        [ResponseType(typeof(Submission))]
        public async Task<IHttpActionResult> PostSubmission(Submission submission)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            string contentType = "";
            var request = HttpWebRequest.Create(submission.Url) as HttpWebRequest;
            if (request != null)
            {
                var response = request.GetResponse() as HttpWebResponse;

                if (response != null)
                    contentType = response.ContentType;
            }

            if (contentType.Contains("video")) {
                submission.Type = SubmissionTypes.Video;
            }

            if (submission.Type == SubmissionTypes.Spotify ||
                submission.Type == SubmissionTypes.YouTube)
            {
                submission.IsHosted = false;
            }

            controller.GetThumbnail(ref submission, contentType);
            var userId = User.Identity.GetUserId();
            submission.Author = db.Users.Find(userId);

            db.Submissions.Add(submission);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = submission.Id }, submission);
        }

        // DELETE: api/Submissions/5
        [Authorize]
        [ResponseType(typeof(Submission))]
        public async Task<IHttpActionResult> DeleteSubmission(int id)
        {
            Submission submission = await db.Submissions.FindAsync(id);
            if (submission == null)
            {
                return NotFound();
            }

            if (submission.Author.Id != User.Identity.GetUserId() &&
                !User.IsInRole("Admins"))
            {
                return Unauthorized();
            }

            controller.RemoveImages(submission);

            db.Submissions.Remove(submission);
            await db.SaveChangesAsync();

            return Ok(submission);
        }

        [Authorize]
        [HttpPost]
        [Route("api/Submissions/{id}/Favorite")]
        public async Task<IHttpActionResult> Favorite(int id, string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var user = db.Users.Find(userId);
                var favorite = db.Favorites.Find(userId, id);
                var submission = db.Submissions.Find(id);

                if (user == null || submission == null)
                {
                    return BadRequest();
                }

                if (favorite != null) // unfavorite this submission
                {
                    db.Favorites.Remove(favorite);
                    submission.Score--;
                }
                else // favorite this submission
                {
                    favorite = db.Favorites.Create();
                    favorite.Submission = submission;
                    favorite.User = db.Users.Find(userId);

                    db.Favorites.Add(favorite);
                    submission.Score++;
                }

                await db.SaveChangesAsync();

                return Ok(submission);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
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

        private bool SubmissionExists(int id)
        {
            return db.Submissions.Count(e => e.Id == id) > 0;
        }
    }
}