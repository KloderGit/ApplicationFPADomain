using System;
using System.Collections.Generic;

namespace Domain.Models.Crm.Fields
{
    public class Field
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<FieldValue> Values { get; set; }

        public bool? IsSystem { get; set; }
    }

    public class FieldValue
    {
        public string Value { get; set; }

        public Int32? @Enum { get; set; }
    }
}
