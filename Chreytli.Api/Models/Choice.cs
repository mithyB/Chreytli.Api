using System.ComponentModel.DataAnnotations.Schema;

namespace Chreytli.Api.Models
{
    public class Choice : Entity
    {
        public string Text { get; set; }

        public int Votes { get; set; }

        public virtual Poll Poll { get; set; }
    }
}