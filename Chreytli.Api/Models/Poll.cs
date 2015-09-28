using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Chreytli.Api.Models
{
    public class Poll : AuthorEntity
    {
        public string Title { get; set; }

        public bool MultipleChoice { get; set; }

        public int TotalVotes { get; set; }

        public virtual List<Choice> Choices { get; set; }

        [NotMapped]
        public bool IsVoted { get; set; }
    }
}