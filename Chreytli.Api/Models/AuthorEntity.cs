using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Chreytli.Api.Models
{
    public class AuthorEntity : Entity
    {
        public string AuthorId { get; set; }

        [NotMapped]
        public object Author { get; set; }

        public DateTime Date { get; set; }
    }
}