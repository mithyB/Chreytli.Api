using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chreytli.Api.Models
{
    public class Poll : AuthorEntity
    {
        public string Title { get; set; }

        public bool MultipleChoice { get; set; }

        public int TotalVotes { get; set; }

        public virtual ICollection<Choice> Choices { get; set; }
        
        public bool IsVoted { get; set; }
    }
}