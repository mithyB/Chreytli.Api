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

namespace Chreytli.Api.Controllers
{
    public class SubmissionsController : ApiController
    {
        private ChreytliApiContext db = new ChreytliApiContext();

        // GET: api/Submissions
        public IQueryable<Submission> GetSubmissions()
        {
            return db.Submissions;
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

            db.Submissions.Add(submission);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = submission.Id }, submission);
        }

        // DELETE: api/Submissions/5
        [Authorize(Roles = "Admin")]
        [ResponseType(typeof(Submission))]
        public async Task<IHttpActionResult> DeleteSubmission(int id)
        {
            Submission submission = await db.Submissions.FindAsync(id);
            if (submission == null)
            {
                return NotFound();
            }

            db.Submissions.Remove(submission);
            await db.SaveChangesAsync();

            return Ok(submission);
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