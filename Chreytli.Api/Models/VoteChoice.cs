namespace Chreytli.Api.Models
{
    public class VoteChoice
    {
        public int VoteId { get; set; }

        public int ChoiceId { get; set; }

        public virtual Vote Vote { get; set; }

        public virtual Choice Choice { get; set; }
    }
}