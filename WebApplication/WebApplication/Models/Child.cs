using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication.Models
{
    public class Child
    {
        public string Name { get; set; }
        public Guid InternalId { get; set; }
        public Guid ParentId { get; set; }
        public virtual Parent Parent { get; set; }

        [NotMapped]
        public Guid Id => InternalId;
    }
}