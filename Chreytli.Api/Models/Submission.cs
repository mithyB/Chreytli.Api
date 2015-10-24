using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Chreytli.Api.Models
{
    public class Submission : AuthorEntity
    {
        [NotMapped]
        public bool IsFavorite { get; set; }

        public string Img { get; set; }

        public string Url { get; set; }

        public int Score { get; set; }

        public bool IsHosted { get; set; }

        public SubmissionTypes Type { get; set; }

        public TagType Tag { get; set; }
    }

    public enum SubmissionTypes 
    {
        Image = 0,
        YouTube = 1,
        Spotify = 2,
        Video = 3
    }

    public enum TagType
    {
        Sfw = 0,  // Safe for work
        Nsfw = 1, // Not safe for work
        Nsfl = 2  // Not safe for life
    }
}