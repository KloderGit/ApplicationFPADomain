using Common.Configuration.Crm;
using Common.Extensions;
using Common.Extensions.Models.Crm;
using Common.Logging;
using Domain.Models.Crm;
using Library1C;
using LibraryAmoCRM;
using LibraryAmoCRM.Configuration;
using LibraryAmoCRM.Infarstructure.QueryParams;
using LibraryAmoCRM.Models;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using WebApiBusinessLogic.Models.Crm;

namespace WebApiBusinessLogic.Infrastructure.CrmDoEventActions
{
    public class UpdatePhone : DoCrmActionBase
    {
        TypeAdapterConfig mapper;

        public UpdatePhone(DataManager amocrm, UnitOfWork database, CrmEventTypes @Events, TypeAdapterConfig mapper)
            :base (amocrm, database)
        {
            Events.Update += DoAction;
            Events.Add += DoAction;

            this.mapper = mapper;
        }

        public async override void DoAction(object sender, CrmEvent e)
        {
            if (e.Entity != "contacts" || String.IsNullOrEmpty(e.EntityId) || e.ContactType != "contact") return;

            try
            {
                var amoUser = amoManager.Contacts.Get().SetParam(prm => prm.Id = int.Parse(e.EntityId))
                                .Execute();

                var contact = amoUser.Result.Adapt<IEnumerable<Contact>>(mapper).FirstOrDefault();

                foreach (var phone in contact.Phones())
                {
                    if (!(phone.Value == phone.Value.LeaveJustDigits()))
                    {
                        contact.Phones(phone.Key, phone.Value.LeaveJustDigits());
                    }
                }

                if (contact.ChangeValueDelegate != null)
                {
                    var dto = contact.GetChanges().Adapt<ContactDTO>(mapper);
                    await amoManager.Contacts.Update(dto);
                }

            }
            catch (Exception ex)
            {
                var info = new MessageLocation(this)
                {
                    Metod = MethodBase.GetCurrentMethod().Name
                };

                //logger.Error("Ошибка, {@Location}", info);
            }

        }

    }
}
