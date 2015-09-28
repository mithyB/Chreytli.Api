using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Chreytli.Api.Models
{
    public class Event : AuthorEntity
    {
        [Key]
        new public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool AllDay { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; } 
    }
}