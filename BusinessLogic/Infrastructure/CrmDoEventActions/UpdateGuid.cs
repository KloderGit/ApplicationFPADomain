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

namespace WebApiBusinessLogic.Infrastructure.CrmDoEventActions
{
    public class UpdateGuid : DoCrmActionBase
    {
        TypeAdapterConfig mapper;
        ILogger logger;

        public UpdateGuid(DataManager amocrm, UnitOfWork database, CrmEventTypes @Events, TypeAdapterConfig mapper, ILogger logger)
            : base (amocrm, database)
        {
            Events.Update += DoAction;
            Events.Add += DoAction;

            this.mapper = mapper;
            this.logger = logger;
        }

        public async override void DoAction(object sender, CrmEvent e)
        {
            if (e.Entity != "contacts" || String.IsNullOrEmpty(e.EntityId) || e.ContactType != "contact") return;

            try
            {
                var amoUser = amoManager.Contacts.Get().SetParam(prm => prm.Id = int.Parse(e.EntityId))
                                .Execute()
                                    .Result
                                    .FirstOrDefault();

                var contact = amoUser.Adapt<Contact>(mapper);

                var hasGuid = contact.Guid();

                if (!String.IsNullOrEmpty(hasGuid)) return;

                var guid = await new LookForContact(database, logger).Find(contact);

                if (!String.IsNullOrEmpty(guid))
                {
                    contact.Guid(guid);

                    await amoManager.Contacts.Update(
                        contact.GetChanges().Adapt<ContactDTO>(mapper)
                    );

                    logger.Information("Обновление Guid - {Guid}, для пользователя Id - {User}", guid, amoUser.Id);
                }

            }
            catch (Exception ex)
            {
                var info = new MessageLocation(this)
                {
                    Metod = MethodBase.GetCurrentMethod().Name
                };

                logger.Error("Ошибка, {@Message}, По адресу - {@Location}", ex.Message, info);
            }
        }
    }
}
