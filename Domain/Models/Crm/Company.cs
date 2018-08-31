using Domain.Interfaces;
using Domain.Models.Crm.Fields;
using System;
using System.Collections.Generic;

namespace Domain.Models.Crm
{
    public class Company : IEntityId
    {
        public Int32 Id { get; set; }

        public Int32? ResponsibleUserId { get; set; }

        public Int32? CreatedBy { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public Int32? AccountId { get; set; }

        public Int32? GroupId { get; set; }

        public string Name { get; set; }

        public DateTime? ClosestTaskAt { get; set; }

        public int? UpdatedBy { get; set; }

        public IEnumerable<Tag> Tags { get; set; }

        public IEnumerable<Field> Fields { get; set; }

        public IEnumerable<Lead> Leads { get; set; }

        public IEnumerable<Contact> Contacts { get; set; }
    }
}