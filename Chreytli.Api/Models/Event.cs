using System;
using System.ComponentModel.DataAnnotations;

namespace Chreytli.Api.Models
{
    public class Event : AuthorEntity
    {
        new public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool AllDay { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; } 
    }
}