using Common.Extensions.Models.Crm;
using Common.Mapping;
using Domain.Models.Crm;
using Library1C;
using LibraryAmoCRM;
using LibraryAmoCRM.Configuration;
using LibraryAmoCRM.Infarstructure.QueryParams;
using LibraryAmoCRM.Interfaces;
using LibraryAmoCRM.Models;
using Mapster;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using ServiceLibraryNeoClient.DTO;
using ServiceLibraryNeoClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WebApiBusinessLogic.Infrastructure.CrmDoEventActions;
using WebApiBusinessLogic.Models.Crm;
using WebApiBusinessLogic.Utils.Mapster;
using ServiceLibraryNeoClient.Implements;
using Common.Extensions;
using WebApiBusinessLogic.Infrastructure.Actions;
using WebApiBusinessLogic.Models.Site;
using Common.DTO.Service1C;
using WebApiBusinessLogic.Infrastructure.Helpers;

namespace WebApiBusinessLogic
{
    public class BusinessLogic
    {
        ILogger logger;
        TypeAdapterConfig mapper;

        LibraryAmoCRM.DataManager amocrm;
        UnitOfWork database;
        ServiceLibraryNeoClient.Implements.DataManager neodatabase;

        CrmEventTypes eventsType = new CrmEventTypes();

        UpdateGuid updGuid;
        UpdatePhone updPhone;
        CreateUser CreateUser;

        public BusinessLogic(ILogger logger, IConfiguration configuration, TypeAdapterConfig mapping,
            LibraryAmoCRM.DataManager amoManager, UnitOfWork service1C)
        {
            this.logger = logger;   // Логи

            this.mapper = mapping;  // Maps
                new RegisterMaps(mapper);

            this.amocrm = amoManager;   // Amo
            this.database = service1C;  // 1C

            //neodatabase = neo;  // neo

            // Events
            updGuid = new UpdateGuid(amocrm, database, eventsType, mapper, logger);
            updPhone = new UpdatePhone(amocrm, eventsType, mapper, logger);
            //CreateUser = new CreateUser(amocrm, database, eventsType, mapper, logger);

            //new RegisterMapsterConfig();

        }

        public void GetEvent(CrmEvent item)
        {
            foreach (var evnt in item.Events)
            {
                switch (evnt.Event)
                {
                    case "add":
                        eventsType.OnAdd(item);      // !!!!!
                        break;
                    case "update":
                        eventsType.OnUpdate(item);
                        break;
                    case "ChangeResponsibleUser":
                        eventsType.OnResponsible(item);
                        break;
                    case "delete":
                        eventsType.OnDelete(item);
                        break;
                    case "note":
                        eventsType.OnNote(item);
                        break;
                    case "status":
                        eventsType.OnStatus(item);
                        break;
                    default:
                        Console.WriteLine("Default case");
                        break;
                }
            }
        }


        public string GetProgramsListForAmo()
        {
            //var programs = neo.Value.Programs.GetList().Where( x => x.Type == "Программа обучения").Where( x=>x.Active );


            var cont = amocrm.Contacts.Get().SetParam(x => x.Phone = "9031453412").Execute().Result;

            var ertert = cont.FirstOrDefault().Adapt<Contact>(mapper);

            var sdfsdf = ertert.Phones();

            //var list = programs.Select( it => new { Name = it.Title, Guid = it.Guid, Type = it.Type } );

            //return JsonConvert.SerializeObject(list);
            return "";
        }


        public async Task<IEnumerable<LeadDTO>> GetLeadsByStatus(int status)
        {
            var sd = new LeadDTO();
            sd.ResponsibleUserId = (int)ResponsibleUserEnum.Анастасия_Столовая;

            return await amocrm.Leads.Get().SetParam(x => x.Status = status).Execute();
        }

        private MethodInfo GetMetod(Type genericType)
        {
            return this.GetType().GetMethod("MetodGet").MakeGenericMethod(genericType);
        }

        //private Func<IAcceptParams> MetodGet<T>() where T : CoreDTO
        //{
        //    var result = amocrm.Repository<T>();
        //    return result.Get;
        //}

        //private Action<T> MetodUpdate<T>() where T : CoreDTO
        //{
        //    var result = amocrm.Repository<T>();
        //    return result.Update;
        //}



        public  bool LookForAmoUser(IEnumerable<string> phone, string email)
        {
            //var user = amocrm.Contacts.Get().SetParam(x => x.Phone = phone.LeaveJustDigits()).Execute().Result;
            //var user2 = amocrm.Contacts.Get().SetParam(x => x.Query = email.ClearEmail()).Execute().Result;

            var userAction = new UserAmoCRM(amocrm, mapper, logger);

            var contactGuid = userAction.FindContact(phone).Result;

            var lead = amocrm.Leads.Get().SetParam(p => p.Query = contactGuid.Name).Execute().Result;

            return true;
        }


        public async Task<int> CreateLeadFormSite(SignUpForEvent item)
        {
            var userAction = new UserAmoCRM(amocrm, mapper, logger);

            Contact contact = null;

            contact = userAction.FindContact(item.ContactPhones).Result;
                if (contact == null) contact = userAction.FindContact(item.ContactEmails).Result;

            Lead lead = null;

            if (contact != null)
            {
                var query = await amocrm.Leads.Get().SetParam(p => p.Query = contact.Name).Execute();
                var result = query.Adapt<IEnumerable<Lead>>(mapper);
                var userLeads = result?.Where(l => l.MainContact.Id == contact.Id);
                lead = userLeads?.FirstOrDefault(l => l.Name.ToUpper().Trim().Contains(item.LeadName.ToUpper().Trim()));
            }

            var types1 = amocrm.Account.Embedded.CustomFields.Leads[66349].Enums;
            var types2 = amocrm.Account.Embedded.CustomFields.Leads[227457].Enums;
            var types = types2.Union(types1).ToDictionary(pair => pair.Key, pair => pair.Value);

            FormDTOBuilder builder = new FormDTOBuilder(contact, lead);

            try
            {
                builder.ContactName(item.ContactName)
                    .Phone(item.ContactPhones)
                    .Email(item.ContactEmails)
                    .City(item.ContactCity)
                    .LeadName(types, item.EventType, item.LeadName)
                    .EducationType(item.LeadType)
                    .Price(item.LeadPrice)
                    .DateOfEvent(item.LeadDate)
                    .LeadGuid(item.LeadGuid);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка при преобразовании запроса с сайта в Модели BL");
                throw new ArgumentException();
            }

            contact = (Contact)builder;
            lead = (Lead)builder;


            if (contact.Id != 0 & lead.Id !=0 )
            {
                var queryCreateLead = await amocrm.Leads.Add(((Lead)builder).Adapt<LeadDTO>(mapper));

                var task = new TaskDTO()
                {
                    ElementId = queryCreateLead == null ? lead.Id : queryCreateLead.Id,
                    ElementType = (int)ElementTypeEnum.Сделка,
                    CompleteTillAt = DateTime.Today,
                    TaskType = 965749,
                    Text = @"Пользователь оставил повторную заявку на это мероприятие. Проверить на дубли."
                };

                var queryCreateTask = await amocrm.Tasks.Add(task);

                return queryCreateLead.Id.Value;
            }


            if (contact.Id != 0 & lead.Id == 0)
            {
                ((Lead)builder).Contacts = new List<Contact> { new Contact { Id = ((Contact)builder).Id } };
                var queryCreateLead = await amocrm.Leads.Add(((Lead)builder).Adapt<LeadDTO>(mapper));

                return queryCreateLead.Id.Value;
            }

            if (contact.Id == 0 & lead.Id == 0)
            {
               var queryCreateContact = await amocrm.Contacts.Add(((Contact)builder).Adapt<ContactDTO>(mapper));

               ((Lead)builder).Contacts = new List<Contact> { new Contact { Id = queryCreateContact.Id.Value } };

               var queryCreateLead = await amocrm.Leads.Add(((Lead)builder).Adapt<LeadDTO>(mapper));

               return queryCreateLead.Id.Value;
            }

            return 0;
        }

    }
}
