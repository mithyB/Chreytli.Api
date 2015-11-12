using System;
using System.ComponentModel.DataAnnotations;

namespace Chreytli.Api.Models
{
    public class Event
    {
        public Guid Id { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public DateTime Date { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool AllDay { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; } 
    }
}