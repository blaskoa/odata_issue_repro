using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication.Models
{
    public class Parent
    {
        public string Name { get; set; }
        public Guid InternalId { get; set; }

        public virtual ICollection<Child> Children { get; set; }

        [NotMapped]
        public Guid Id => InternalId;
    }
}