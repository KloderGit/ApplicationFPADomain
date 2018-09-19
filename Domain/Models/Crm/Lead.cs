using Domain.Interfaces;
using Domain.Models.Crm.Fields;
using Domain.Models.Crm.Parent;
using System;
using System.Collections.Generic;

namespace Domain.Models.Crm
{
    public class Lead : EntityCore, IEntityId
    {
        public string Name { get; set; }

        public Int32? Status { get; set; }

        public Int32? Price { get; set; }

        public Int32? LossReason { get; set; }

        public DateTime? ClosestTaskAt { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? ClosedAt { get; set; }

        public List<Tag> Tags { get; set; }

        public List<Field> Fields { get; set; }

        public Company Company { get; set; }

        public List<Contact> Contacts { get; set; }

        public Contact MainContact { get; set; }

        public Pipeline Pipeline { get; set; }

        public Action<Lead> ChangeValueDelegate;

        public Lead GetChanges()
        {
            var leadWithChangedFields = new Lead();
            leadWithChangedFields.Id = this.Id;

            if (ChangeValueDelegate != null) ChangeValueDelegate.Invoke(leadWithChangedFields);

            return leadWithChangedFields;
        }
    }
}