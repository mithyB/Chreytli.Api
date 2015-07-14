using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Chreytli.Api.Models
{
    public class Poll
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public bool MultipleChoice { get; set; }

        public int TotalVotes { get; set; }

        public virtual List<Choice> Choices { get; set; }

        public string AuthorId { get; set; }

        [NotMapped]
        public object Author { get; set; }

        public DateTime Date { get; set; }

        [NotMapped]
        public bool IsVoted { get; set; }
    }
}