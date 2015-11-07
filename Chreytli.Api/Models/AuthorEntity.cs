using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chreytli.Api.Models
{
    public class AuthorEntity : Entity
    {
        public virtual ApplicationUser Author { get; set; }

        public DateTime Date { get; set; }
    }
}