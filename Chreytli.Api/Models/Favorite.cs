namespace Chreytli.Api.Models
{
    public class Favorite
    {
        public string UserId { get; set; }

        public int SubmissionId { get; set; }

        public virtual ApplicationUser User { get; set; }
        
        public virtual Submission Submission { get; set; }
    }
}