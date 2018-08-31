using Domain.Interfaces;
using Domain.Models.Crm.Fields;
using System;
using System.Collections.Generic;

namespace Domain.Models.Crm
{
    public class Lead : IEntityId
    {
        public Int32 Id { get; set; }

        public Int32? ResponsibleUserId { get; set; }

        public Int32? CreatedBy { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public Int32? AccountId { get; set; }

        public Int32? GroupId { get; set; }

        public string Name { get; set; }

        public Int32? Status { get; set; }

        public Int32? Price { get; set; }

        public Int32? LossReason { get; set; }

        public DateTime? ClosestTaskAt { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? ClosedAt { get; set; }

        public IEnumerable<Tag> Tags { get; set; }

        public IEnumerable<Field> CustomFields { get; set; }

        public Company Company { get; set; }

        public IEnumerable<Contact> Contacts { get; set; }

        public Contact MainContact { get; set; }

        public Pipeline Pipeline { get; set; }
    }
}