using Common.Extensions.Models.Crm;
using Common.Interfaces;
using Domain.Models.Crm;
using Library1C;
using LibraryAmoCRM.Infarstructure.QueryParams;
using LibraryAmoCRM.Interfaces;
using LibraryAmoCRM.Models;
using Mapster;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApiBusinessLogic.Infrastructure.CrmDoEventActions.Shared;
using WebApiBusinessLogic.Models.Crm;

namespace WebApiBusinessLogic.Infrastructure.CrmDoEventActions
{
    public class UpdateGuid
    {
        IDataManager crm;
        UnitOfWork database;

        TypeAdapterConfig mapper;

        ILoggerFactory loggerFactory;
        ILogger currentLogger;

        public UpdateGuid(IDataManager amocrm, UnitOfWork database, CrmEventTypes @Events, TypeAdapterConfig mapper, ILoggerFactory loggerFactory)
        {
            this.mapper = mapper;

            this.loggerFactory = loggerFactory;
            this.currentLogger = loggerFactory.CreateLogger(this.ToString());

            this.database = database;
            this.crm = amocrm;

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
                amoUser = await crm.Contacts.Get().Filter(prm => prm.Id = int.Parse(e.EntityId)).Execute();
                    if (amoUser == null) throw new NullReferenceException( "Контакт [ Id -" + e.EntityId + " ] не найден в CRM" );

                contact = amoUser.FirstOrDefault().Adapt<Contact>(mapper);

                var hasGuid = contact.Guid();
                    if (!String.IsNullOrEmpty(hasGuid)) return;

                var guid = await new LookForContact(database, loggerFactory).Find(contact);

                if (!String.IsNullOrEmpty(guid))
                {
                    contact.Guid(guid);

                    await crm.Contacts.Update(
                        contact.GetChanges().Adapt<ContactDTO>(mapper)
                    );

                    currentLogger.LogInformation("Обновление Guid - {Guid}, для пользователя Id - {User}", guid, contact.Id);
                }

            }
            catch (NullReferenceException ex)
            {
                currentLogger.LogDebug( ex, "Ошибка, нулевое значение {@Contacts}", contact, amoUser );
                return;
            }
            catch (Exception ex)
            {
                currentLogger.LogError(ex, "Ошибка обновления пользователя. [{@Id}]", contact.Id );
            }
        }
    }
}
