using Domain.Models.Crm;
using Library1C;
using LibraryAmoCRM;
using LibraryAmoCRM.Infarstructure.QueryParams;
using Mapster;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApiBusinessLogic.Models.Crm;

namespace WebApiBusinessLogic.Infrastructure.CrmDoEventActions
{
    public class CreateUser 
    {
        DataManager amoManager;
        UnitOfWork database;

        TypeAdapterConfig mapper;
        ILogger logger;

        public CreateUser(DataManager amocrm, UnitOfWork service1C, CrmEventTypes @Events, TypeAdapterConfig mapper, ILogger logger)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.amoManager = amocrm;
            this.database = service1C;

            Events.Status += DoAction;
        }

        public async void DoAction(object sender, CrmEvent e)
        {
            if (e.Entity != "leads" || String.IsNullOrEmpty(e.EntityId)) return;

            var userActions = new Actions.User1C(database, logger);

            try
            {
                var queryLead = await amoManager.Leads.Get().SetParam(i => i.Id = int.Parse(e.EntityId)).Execute();
                var lead = queryLead.FirstOrDefault().Adapt<Lead>(mapper);

                var queryContact = await amoManager.Contacts.Get().SetParam(i => i.Id = lead.MainContact.Id).Execute();
                var contact = queryContact.FirstOrDefault().Adapt<Contact>(mapper);

                var lkl = await userActions.Create(contact);
            }
            catch (Exception ex)
            {
            }
            
        }

    }
}
