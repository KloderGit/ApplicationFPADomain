using Library1C;
using LibraryAmoCRM;
using Mapster;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using WebApiBusinessLogic.Models.Crm;

namespace WebApiBusinessLogic.Infrastructure.CrmDoEventActions
{
    public class CreatePersonsContractInto1C
    {
        DataManager amoManager;
        UnitOfWork database;

        TypeAdapterConfig mapper;
        ILogger logger;

        public CreatePersonsContractInto1C(DataManager amocrm, UnitOfWork service1C, CrmEventTypes @Events, TypeAdapterConfig mapper, ILogger logger)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.amoManager = amocrm;
            this.database = service1C;

            //Events.Status += DoAction;
        }
    }
}
