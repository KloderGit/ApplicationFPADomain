using Common.Configuration.Crm;
using Common.Extensions;
using Common.Extensions.Models.Crm;
using Domain.Models.Crm;
using Library1C;
using LibraryAmoCRM;
using LibraryAmoCRM.Infarstructure.QueryParams;
using LibraryAmoCRM.Models;
using Mapster;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApiBusinessLogic.Models.Crm;

namespace WebApiBusinessLogic.Infrastructure.CrmDoEventActions
{
    public class UpdateGuidByChangeLeadStatus : DoCrmActionBase
    {
        ILogger logger;
        TypeAdapterConfig mapper;

        public UpdateGuidByChangeLeadStatus(DataManager amocrm, UnitOfWork database, CrmEventTypes @Events, TypeAdapterConfig mapper, ILogger logger)
        : base(amocrm, database)
        {
            //Events.Update += DoAction;
            //Events.Add += DoAction;

            this.mapper = mapper;
            this.logger = logger;
        }

        public async override void DoAction(object sender, CrmEvent e)
        {
            if (e.Entity != "leads" || String.IsNullOrEmpty(e.EntityId)) return;

            Lead lead;

            // Получить сделку и проверить наличие Guid
            try
            {
                var query = await amoManager.Leads.Get().SetParam(x => x.Id = int.Parse(e.EntityId)).Execute();

                lead = query.FirstOrDefault().Adapt<Lead>(mapper);

                if (lead == null || lead.Fields.FirstOrDefault(x => x.Id == (int)LeadFieldsEnum.Guid) == null) return;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            try
            {
                IEnumerable<Contact> loadedContacts;

                var query = amoManager.Contacts.Get();

                for (var i = 0; i < lead.Contacts.Count; i++)
                {
                    query = query.SetParam(x => x.Id = lead.Contacts[i].Id);
                }

                var result = await query.Execute();

                loadedContacts = result.Adapt<IEnumerable<Contact>>(mapper);

                for (var i = 0; i < lead.Contacts.Count; i++)
                {
                    lead.Contacts[i] = loadedContacts.FirstOrDefault(x => x.Id == lead.Contacts[i].Id);
                }
            }
            catch (Exception ex)
            {

            }

            if (lead.Contacts == null || lead.Contacts.Where( x => String.IsNullOrEmpty(x.Guid() ) ).Count() > 0) return;

            try
            {
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }
    }
}
