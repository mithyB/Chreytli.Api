using System.Collections.Generic;

namespace Chreytli.Api.Models
{
    public class Vote : Entity
    {
        public virtual ApplicationUser User { get; set; }

        public virtual Poll Poll { get; set; }

        public virtual ICollection<VoteChoice> VoteChoices { get; set; }
    }
}