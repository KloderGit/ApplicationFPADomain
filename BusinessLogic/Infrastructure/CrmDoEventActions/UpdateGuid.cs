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

namespace WebApiBusinessLogic.Infrastructure.CrmDoEventActions
{
    public class UpdateGuid : DoCrmActionBase
    {
        public UpdateGuid(DataManager amocrm, UnitOfWork database, CrmEventTypes @Events, [FromServices]ILogger logger)
            :base (amocrm, database)
        {
            Events.Update += DoAction;
            Events.Add += DoAction;
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


                //var sdff = new CreateUser1C(database).Create(amoUser);


                var hasGuid = amoUser.CustomFields?.FirstOrDefault(it => it.Id == 571611)?.Values.FirstOrDefault().Value;

                if (!String.IsNullOrEmpty(hasGuid)) return;


                var guid = await new LookForContactGuid(database).Find(amoUser);


                if (!String.IsNullOrEmpty(guid))
                {
                    var updateItem = new ContactDTO
                    {
                        Id = amoUser.Id,
                        UpdatedAt = DateTime.Today,
                        CustomFields = new[] {
                            new CustomField{
                                Id = 571611,
                                Values = new[] {
                                    new CustomFieldValue { Value = guid }
                                }
                            }
                        }
                    };

                    await amoManager.Contacts.Update(updateItem);

                    Log.Logger = new LoggerConfiguration()
                    .WriteTo.Seq("http://logs.fitness-pro.ru:5341")
                    .CreateLogger();

                    Log.Information("Обновление Guid - {Guid}, для пользователя Id - {User}", guid, amoUser.Id);
                }

            }
            catch (Exception ex)
            {

            }
        }
    }
}
