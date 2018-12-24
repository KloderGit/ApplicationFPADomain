using Domain.Models.Crm.Fields;
using Domain.Models.Crm.Parent;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Models.Crm
{
    public class Contact : EntityMember
    {
        public int? UpdatedBy { get; set; }

        public List<Lead> Leads { get; set; }

        public Company Company { get; set; }

        public Action<Contact> ChangeValueDelegate;

        public Contact GetChanges()
        {
            var contactWithChangedFields = new Contact();
            contactWithChangedFields.Id = this.Id;

            if (ChangeValueDelegate != null) ChangeValueDelegate.Invoke(contactWithChangedFields);

            return contactWithChangedFields;
        }

        public void SetGuid(string value)
        {
            Fields = Fields ?? new List<Field>();
            if (!Fields.Any(x => x.Id == 571611)) Fields.Add( new Field { Id = 571611 } );

            Fields.FirstOrDefault(x => x.Id == 571611).Values = new List<FieldValue> { new FieldValue { Value = value } };
        }


    }
}