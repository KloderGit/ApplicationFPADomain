using Domain.Models.Crm.Fields;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models.Crm.Parent
{
    public class EntityMember : EntityCore
    {
        public string Name { get; set; }

        public DateTime? ClosestTaskAt { get; set; }

        public IEnumerable<Tag> Tags { get; set; }

        public List<Field> Fields { get; set; }
    }
}
