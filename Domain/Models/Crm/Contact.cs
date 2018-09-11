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

        public IEnumerable<Lead> Leads { get; set; }

        public Company Company { get; set; }


        public IEnumerable<FieldValue> GetFieldByID(int id)
        {
            return Fields.FirstOrDefault(x => x.Id == id).Values;
        }

        public void SetField(int id, string value, int? @enum)
        {
            try
            {
                if (Fields.FirstOrDefault(x => x.Id == id) == null) { return; }

                var currentField = GetFieldByID(id);

                var currentValues = currentField.Select(x => x.Value);

                if (!currentValues.Contains(value))
                {
                    var tttt = new FieldValue { Value = value, Enum = @enum };

                    this.Fields.FirstOrDefault(x => x.Id == id).Values.Add(tttt);

                }
            }
            catch (Exception ex)
            { }
        }

    }
}