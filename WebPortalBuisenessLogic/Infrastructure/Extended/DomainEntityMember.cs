using Common.Configuration.Crm;
using Domain.Models.Crm.Fields;
using Domain.Models.Crm.Parent;
using LibraryAmoCRM.Configuration;
using LibraryAmoCRM.Infarstructure.QueryParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebPortalBuisenessLogic.Infrastructure.Extended
{
    public static class DomainEntityMember
    {
        public static IEnumerable<FieldValue> GetField(this EntityMember item, ContactFieldsEnum field)
        {
            var result = item.Fields.Where(x => x.Id == (int)field)?.SelectMany(c=>c.Values);

            return result;
        }
    }
}
