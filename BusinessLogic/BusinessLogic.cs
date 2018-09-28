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
            sd.ResponsibleUserId = (int)Users.Анастасия_Столовая;

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


    }
}
