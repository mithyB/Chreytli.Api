using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Chreytli.Api.Models
{
    public class Choice
    {
        [Key]
        public int Id { get; set; }

        public string Text { get; set; }

        public int Votes { get; set; }

        [ForeignKey("Poll")]
        public int PollId { get; set; }

        public virtual Poll Poll { get; set; }
    }
}