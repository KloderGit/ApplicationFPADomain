using Common.Extensions;
using Common.Extensions.Models.Crm;
using Common.Interfaces;
using Domain.Models.Crm;
using LibraryAmoCRM;
using LibraryAmoCRM.Infarstructure.QueryParams;
using LibraryAmoCRM.Models;
using Mapster;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiBusinessLogic.Infrastructure.Actions
{
    public class UserAmoCRM
    {
        ILoggerService logger;
        DataManager amoManager;
        TypeAdapterConfig mapper;

        public UserAmoCRM(DataManager amoManager, TypeAdapterConfig mapper, ILoggerService logger)
        {
            this.logger = logger;
            this.amoManager = amoManager;
            this.mapper = mapper;
        }

        public async Task<string> GetContactGuid(IEnumerable<string> phones, IEnumerable<string> emails)
        {
            string guid = String.Empty;
            
            var findByPhones = await FindContact(ClearPhones(phones));
            guid = findByPhones.Guid();

            if (!String.IsNullOrEmpty(guid)) return guid;

            var findByEmails = await FindContact(ClearEmails(emails));
            guid = findByEmails.Guid();

            return guid;

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

        public async Task<Contact> FindContact(IEnumerable<string> queryParams)
        {
            if (queryParams == null) return null;

            var query = amoManager.Contacts.Get();

            foreach (var param in queryParams)
            {
                query.SetParam(i=>i.Query = param);
            }
            
            var result = await query.Execute();

            return result?.FirstOrDefault().Adapt<Contact>(mapper);
        }

        public async Task<Contact> FindContact(string queryParam)
        {
            if ( String.IsNullOrEmpty(queryParam) ) return null;

            var query = amoManager.Contacts.Get().SetParam(p=>p.Query = queryParam);
            var result = await query.Execute();

            return result?.FirstOrDefault().Adapt<Contact>(mapper);
        }
    }
}
