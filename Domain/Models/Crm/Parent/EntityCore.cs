using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models.Crm.Parent
{
    public class EntityCore : IEntityId
    {
        public Int32 Id { get; set; }

        public Int32? ResponsibleUserId { get; set; }

        public Int32? CreatedBy { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public Int32? AccountId { get; set; }

        public Int32? GroupId { get; set; }

        public Action<Contact> ChangeValueDelegate;
    }
}
