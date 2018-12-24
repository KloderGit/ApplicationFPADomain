using Common.Extensions;
using Common.Extensions.Models.Crm;
using Common.Interfaces;
using Domain.Models.Crm;
using LibraryAmoCRM;
using LibraryAmoCRM.Infarstructure.QueryParams;
using LibraryAmoCRM.Interfaces;
using LibraryAmoCRM.Models;
using Mapster;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiBusinessLogic.Infrastructure.Actions
{
    public class AmoCRMCommonActions
    {
        ILoggerService logger;
        IDataManager amoManager;
        TypeAdapterConfig mapper;

        public AmoCRMCommonActions(IDataManager amoManager, TypeAdapterConfig mapper, ILoggerService logger)
        {
            this.logger = logger;
            this.amoManager = amoManager;
            this.mapper = mapper;
        }

        public async Task<Contact> LookForContact(IEnumerable<string> phones, IEnumerable<string> emails, string guid)
        {
            var query = await FindContactByGUID( guid );
            if (query != null) return query;

            query = await FindContact(ClearPhones(phones));
            if (query != null) return query;

            query = await FindContact(ClearEmails(emails));

            return query;

            IEnumerable<string> ClearPhones(IEnumerable<string> values)
            {
                var array = values.ToList();
                for (var i = 0; i < array.Count(); i++) { array[i] = array[i].LeaveJustDigits(); }
                return array;
            }
            IEnumerable<string> ClearEmails(IEnumerable<string> values)
            {
                var array = values.ToList();
                for (var i = 0; i < array.Count(); i++) { array[i] = array[i].ClearEmail(); }
                return array;
            }
        }

        public async Task<Contact> LookForContact(string phone, string email, string guid)
        {
            var query = await FindContactByGUID( guid );
            if (query != null) return query;

            query = await FindContact( phone.LeaveJustDigits() );
            if (query != null) return query;

            query = await FindContact( email.ClearEmail() );

            return query;
        }

        public async Task<Contact> FindContact(IEnumerable<string> queryParams)
        {
            if (queryParams == null) return null;

            var query = amoManager.Contacts.Get();

            foreach (var param in queryParams)
            {
                query.Filter(i=>i.Query = param);
            }
            
            var result = await query.Execute();

            return result?.FirstOrDefault().Adapt<Contact>(mapper);
        }

        public async Task<Contact> FindContact(string queryParam)
        {
            if ( String.IsNullOrEmpty(queryParam) ) return null;

            var query = amoManager.Contacts.Get().Filter(p=>p.Query = queryParam);
            var result = await query.Execute();

            return result?.FirstOrDefault().Adapt<Contact>(mapper);
        }

        public async Task<Contact> FindContactByGUID(string guid)
        {
            if (String.IsNullOrEmpty( guid )) return null;

            var query = amoManager.Contacts.Get().Filter( p => p.Query = guid );
            var result = await query.Execute();

            return result?.FirstOrDefault().Adapt<Contact>( mapper );
        }
    }
}
