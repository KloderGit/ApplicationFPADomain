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

namespace WebApiBusinessLogic
{
    public class BusinessLogic
    {
        ILogger logger;
        TypeAdapterConfig mapper;

        DataManager amocrm;
        UnitOfWork database;

        Lazy<ServiceLibraryNeoClient.Implements.DataManager> neo;

        CrmEventTypes eventsType = new CrmEventTypes();

        UpdateGuid updGuid;
        UpdatePhone updPhone;
        UpdateGuidByChangeLeadStatus updByStatus;

        CreateUser CreateUser;


        public BusinessLogic(ILogger logger, IConfiguration configuration, TypeAdapterConfig mapping)
        {
            this.logger = logger;
            this.mapper = mapping;
                new RegisterMaps(mapper);


            var amoAccount = configuration.GetSection("providers:0:AmoCRM:connection:account:name").Value;
            var amoUser = configuration.GetSection("providers:0:AmoCRM:connection:account:email").Value;
            var amoPass = configuration.GetSection("providers:0:AmoCRM:connection:account:hash").Value;

            var user1C = configuration.GetSection("providers:1:1C:connection:account:user").Value;
            var pass1C = configuration.GetSection("providers:1:1C:connection:account:pass").Value;

            this.amocrm = new DataManager(amoAccount, amoUser, amoPass);

            this.database = new UnitOfWork(user1C, pass1C);

            neo = new Lazy<ServiceLibraryNeoClient.Implements.DataManager>();


            updGuid = new UpdateGuid(amocrm, database, eventsType, mapper, logger);
            updPhone = new UpdatePhone(amocrm, database, eventsType, mapper, logger);
            updByStatus = new UpdateGuidByChangeLeadStatus(amocrm, database, eventsType, mapper, logger);
            CreateUser = new CreateUser(amocrm, database, eventsType, mapper, logger);

            new RegisterMapsterConfig();

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
