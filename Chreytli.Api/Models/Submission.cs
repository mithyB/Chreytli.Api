using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Chreytli.Api.Models
{
    public class Submission
    {
        [Key]
        public int Id { get; set; }

        public string AuthorId { get; set; }

        public string Img { get; set; }

        public string Url { get; set; }

        public DateTime Date { get; set; }

        public int Score { get; set; }

        public SubmissionTypes Type { get; set; }
    }

    public enum SubmissionTypes 
    {
        Image
    }
}