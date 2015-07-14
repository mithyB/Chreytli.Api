using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Chreytli.Api.Models
{
    public class VoteChoice
    {
        [Key, Column(Order = 0)]
        public int VoteId { get; set; }

        [Key, Column(Order = 1)]
        public int ChoiceId { get; set; }
    }
}