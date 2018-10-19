using Common.Extensions.Models.Crm;
using Common.Mapping;
using Domain.Models.Crm;
using Domain.Models.Education;
using Library1C;
using LibraryAmoCRM;
using LibraryAmoCRM.Configuration;
using LibraryAmoCRM.Infarstructure.QueryParams;
using LibraryAmoCRM.Models;
using Mapster;
using Microsoft.Extensions.Configuration;
using Serilog;
using ServiceLibraryNeoClient.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WebPortalBuisenessLogic.Models.Crm;
using WebPortalBuisenessLogic.Models.DataBase;
using WebPortalBuisenessLogic.Utils.Mapster;

namespace WebPortalBuisenessLogic
{
    public class BusinessLogic
    {
        ILogger logger;
        TypeAdapterConfig mapper;

        Lazy<DataManager> amocrm;
        Lazy<UnitOfWork> database;

        ServiceLibraryNeoClient.Implements.DataManager neoDB;

        public BusinessLogic(ILogger logger, IConfiguration configuration, TypeAdapterConfig mapping)
        {
            this.logger = logger;
            this.mapper = mapping;

            var amoAccount = configuration.GetSection("providers:0:AmoCRM:connection:account:name").Value;
            var amoUser = configuration.GetSection("providers:0:AmoCRM:connection:account:email").Value;
            var amoPass = configuration.GetSection("providers:0:AmoCRM:connection:account:hash").Value;

            var user1C = configuration.GetSection("providers:1:1C:connection:account:user").Value;
            var pass1C = configuration.GetSection("providers:1:1C:connection:account:pass").Value;

            this.amocrm = new Lazy<DataManager>(() => new DataManager(amoAccount, amoUser, amoPass));

            this.database = new Lazy<UnitOfWork>(new UnitOfWork(user1C, pass1C));

            this.neoDB = new ServiceLibraryNeoClient.Implements.DataManager( new Uri( "http://localhost:7474/db/data" ), "neo4j", "Kaligula2" );
        }




        public async Task<bool> UpdateLeadAndContact(WizardDTO model)
        {
            var lead = model.Adapt<Lead>(mapper);
            lead.Program(model.Program);

            var leadDTO = lead.GetChanges().Adapt<LeadDTO>(mapper);
            leadDTO.Name = "Сделка с планшета";
            if (model.Program != 0) leadDTO.Name = amocrm.Value.Account.Embedded.CustomFields.Leads.FirstOrDefault(k => k.Key == 227457).Value.Enums.FirstOrDefault(x=>x.Key == model.Program).Value;

            var contact = model.Adapt<Contact>(mapper);
            contact.Phones(PhoneTypeEnum.MOB, model.Phone);
            contact.Email(EmailTypeEnum.PRIV, model.Email);
            contact.Birthday(model.Birthday);
            contact.City(model.City);
            contact.Location(model.Subway);
            contact.Education(model.Education);
            contact.Experience(model.Expirience);
            contact.GroupPart(model.ProgramPart.ToString());

            var contactDTO = contact.GetChanges().Adapt<ContactDTO>(mapper);
            contactDTO.Name = model.Name;

            try
            {
                await amocrm.Value.Contacts.Update(contactDTO);

                await amocrm.Value.Leads.Update(leadDTO);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }


        }

        public async Task<IEnumerable<WizardDTO>> GetPreparedLeads()
        {
            return await GetLeadsByStatus(17793889);
        }

        public async Task<IEnumerable<WizardDTO>> GetLeadsByStatus(int status)
        {
            IEnumerable<WizardDTO> result = new List<WizardDTO>();

            try
            {
                var queryLeads = amocrm.Value.Leads.Get().SetParam(x => x.Status = status).Execute().Result;
                var leads = queryLeads.Adapt<IEnumerable<Lead>>(mapper);

                var ld = leads?.Where(cn => cn.Contacts != null || cn.MainContact != null).ToList();

                for (int xx = 0; xx < ld.Count(); xx++)
                {
                    ld[xx].MainContact = await GetContactById(ld[xx].MainContact.Id);
                }

                result = ld.Adapt<IEnumerable<WizardDTO>>(mapper);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return result;
        }

        public async Task<Contact> GetContactById(int id)
        {
            var contacts = await amocrm.Value.Contacts.Get().SetParam(x => x.Id = id).Execute();
            return contacts.FirstOrDefault().Adapt<Contact>(mapper);
        }

        public async Task<WizardDTO> GetLeadById(int id)
        {
            var queryLead = await amocrm.Value.Leads.Get().SetParam(x => x.Id = id).Execute();
            var lead = queryLead.FirstOrDefault().Adapt<Lead>(mapper);

            lead.MainContact = await GetContactById(lead.MainContact.Id);

            return lead.Adapt<WizardDTO>(mapper);
        }



        public void UpdateEducationDB()
        {
            var updQuery = database.Value.Programs.GetList().Result;

            var array = updQuery.Where(p=>p.active== "Активный" ).Adapt<IEnumerable<EducationProgram>>( mapper );
            var dto = array.Adapt< IEnumerable<ProgramNode>>( mapper );

            foreach (var item in dto)
            {
                neoDB.Programs.Add( item );
            }
        }

        public IEnumerable<EducationProgram> GetDBPrograms()
        {
            var query = neoDB.Programs.GetList().Adapt<IEnumerable<EducationProgram>>( mapper );

            return query;
        }
    }
}