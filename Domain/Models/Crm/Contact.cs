using Domain.Interfaces;
using Domain.Models.Crm.Fields;
using Domain.Models.Crm.Parent;
using LibraryAmoCRM.Configuration;
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

        public Contact GetChanges()
        {
            var contactWithChangedFields = new Contact();
            contactWithChangedFields.Id = this.Id;

            if (ChangeValueDelegate != null) ChangeValueDelegate.Invoke(contactWithChangedFields);

            return contactWithChangedFields;
        }
    }
}