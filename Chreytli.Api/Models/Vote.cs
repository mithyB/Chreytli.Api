using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Chreytli.Api.Models
{
    public class Vote
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        public int PollId { get; set; }

        public List<VoteChoice> VoteChoices { get; set; }
    }
}