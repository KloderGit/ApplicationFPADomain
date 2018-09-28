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
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using WebApiBusinessLogic.Models.Crm;

namespace WebApiBusinessLogic.Infrastructure.CrmDoEventActions
{
    public class UpdatePhone
    {
        DataManager amocrm;

        TypeAdapterConfig mapper;
        ILogger logger;

        public UpdatePhone(DataManager amocrm, CrmEventTypes @Events, TypeAdapterConfig mapper, ILogger logger)
        {
            this.amocrm = amocrm;
            this.mapper = mapper;
            this.logger = logger;

            Events.Update += DoAction;
            Events.Add += DoAction;
        }

        public async void DoAction(object sender, CrmEvent e)
        {
            if (e.Entity != "contacts" || String.IsNullOrEmpty(e.EntityId) || e.ContactType != "contact") return;

            IEnumerable<ContactDTO> amoUser = null;
            Contact contact = null;

            try
            {
                amoUser = await amocrm.Contacts.Get().SetParam(prm => prm.Id = int.Parse(e.EntityId)).Execute();
                contact = amoUser.Adapt<IEnumerable<Contact>>(mapper).FirstOrDefault();
            }
            catch (NullReferenceException ex)
            {
                logger.Error(ex, "Ошибка, нулевое значение {@Contacts}", contact, amoUser);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Запрос пользователя amoCRM окончился неудачей. Событие - {@Event}, {@AmoUser}", e, amoUser, contact);
            }

            if (contact != null)
            {
                foreach (var phone in contact.Phones())
                {
                    if (!(phone.Value == phone.Value.LeaveJustDigits()))
                    {
                        contact.Phones(phone.Key, phone.Value.LeaveJustDigits());
                    }
                }
            }

            try
            {
                if (contact.ChangeValueDelegate != null)
                {
                    var dto = contact.GetChanges().Adapt<ContactDTO>(mapper);
                    await amocrm.Contacts.Update(dto);
                    logger.Information("Обновление Phone для пользователя Id - {User}", contact.Id);
                }
            }
            catch (Exception ex)
            {
                var info = new MessageLocation(this)
                {
                    Metod = MethodBase.GetCurrentMethod().Name
                };

                logger.Error(ex, "Ошибка, {@Location}",info);
            }

        }

    }
}
