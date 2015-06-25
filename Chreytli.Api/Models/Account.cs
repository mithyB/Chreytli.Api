using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Chreytli.Api.Models
{
    public class Account
    {
        [Key]
        public string UserId { get; set; }

        [ForeignKey("UserId"), NotMapped]
        public ApplicationUser User { get; set; }

        public DateTime CreateDate { get; set; }
    }
}