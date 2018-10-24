using Common.Extensions;
using Common.Extensions.Models.Crm;
using Common.Interfaces;
using Common.Logging;
using Domain.Models.Crm;
using LibraryAmoCRM.Infarstructure.QueryParams;
using LibraryAmoCRM.Interfaces;
using LibraryAmoCRM.Models;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WebApiBusinessLogic.Models.Crm;

namespace WebApiBusinessLogic.Infrastructure.CrmDoEventActions
{
    public class UpdatePhone
    {
        TypeAdapterConfig mapper;
        ILoggerService logger;

        IDataManager crm;

        public UpdatePhone(IDataManager crm, CrmEventTypes @Events, TypeAdapterConfig mapper, ILoggerService logger)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.crm = crm;

            Events.Update += DoAction;
            Events.Add += DoAction;
        }

        public async void DoAction(object sender, CrmEvent e)
        {
            if (e.Entity != "contacts" || String.IsNullOrEmpty( e.EntityId ) || e.ContactType != "contact") return;

            IEnumerable<ContactDTO> amoUser = null;
            Contact contact = null;

            try
            {
                amoUser = await crm.Contacts.Get().Filter( f => f.Id = int.Parse( e.EntityId ) ).Execute();
                    if (amoUser == null) throw new NullReferenceException( "Контакт [ Id -" + e.EntityId + " ] не найден в CRM" );

                contact = amoUser.Adapt<IEnumerable<Contact>>( mapper ).FirstOrDefault();
            }
            catch (NullReferenceException ex)
            {
                logger.Debug( ex, "Ошибка, нулевое значение {@Contacts}", contact, amoUser );
                return;
            }
            catch (Exception ex)
            {
                logger.Debug( ex, "Запрос пользователя amoCRM окончился неудачей. Событие - {@Event}, {@AmoUser}", e, amoUser, contact );
                return;
            }

            foreach (var phone in contact.Phones())
            {
                if (!( phone.Value == phone.Value.LeaveJustDigits() ))
                {
                    contact.Phones( phone.Key, phone.Value.LeaveJustDigits() );
                }
            }

            try
            {
                if (contact.ChangeValueDelegate != null)
                {
                    var dto = contact.GetChanges().Adapt<ContactDTO>( mapper );
                    await crm.Contacts.Update( dto );

                    logger.Information( "Обновление Phone для пользователя Id - {User}", contact.Id );
                }
            }
            catch (Exception ex)
            {
                logger.Debug( ex, "Ошибка обновления пользователя. [{@Id}]", contact.Id );
            }

        }

    }
}
