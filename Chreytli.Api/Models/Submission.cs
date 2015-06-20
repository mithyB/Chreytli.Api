using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Chreytli.Api.Models
{
    public class Submission
    {
        [Key]
        public int Id { get; set; }

        public string AuthorId { get; set; }

        [NotMapped]
        public ApplicationUser Author { get; set; }

        [NotMapped]
        public bool IsFavorite { get; set; }

        public string Img { get; set; }

        public string Url { get; set; }

        public DateTime Date { get; set; }

        public int Score { get; set; }

        public SubmissionTypes Type { get; set; }
    }

    public enum SubmissionTypes 
    {
        Image,
        YouTube,
        Spotify
    }
}