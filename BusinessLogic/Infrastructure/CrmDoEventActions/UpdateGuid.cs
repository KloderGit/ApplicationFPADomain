using Library1C;
using LibraryAmoCRM;
using LibraryAmoCRM.Infarstructure.QueryParams;
using LibraryAmoCRM.Models;
using Serilog;
using System;
using System.Linq;
using WebApiBusinessLogic.Infrastructure.CrmDoEventActions.Shared;
using WebApiBusinessLogic.Models.Crm;
using Common.Extensions.Models.Crm;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Domain.Models.Crm;
using Common.Logging;
using System.Reflection;
using Common.Interfaces;
using LibraryAmoCRM.Interfaces;
using System.Collections.Generic;

namespace WebApiBusinessLogic.Infrastructure.CrmDoEventActions
{
    public class UpdateGuid
    {
        IDataManager crm;
        UnitOfWork database;

        TypeAdapterConfig mapper;
        ILoggerService logger;

        public UpdateGuid(IDataManager amocrm, UnitOfWork database, CrmEventTypes @Events, TypeAdapterConfig mapper, ILoggerService logger)
        {
            this.mapper = mapper;
            this.logger = logger;
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

                var guid = await new LookForContact(database, logger).Find(contact);

                if (!String.IsNullOrEmpty(guid))
                {
                    contact.Guid(guid);

                    await crm.Contacts.Update(
                        contact.GetChanges().Adapt<ContactDTO>(mapper)
                    );

                    logger.Information("Обновление Guid - {Guid}, для пользователя Id - {User}", guid, contact.Id);
                }

            }
            catch (NullReferenceException ex)
            {
                logger.Debug( ex, "Ошибка, нулевое значение {@Contacts}", contact, amoUser );
                return;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка обновления пользователя. [{@Id}]", contact.Id );
            }
        }
    }
}
