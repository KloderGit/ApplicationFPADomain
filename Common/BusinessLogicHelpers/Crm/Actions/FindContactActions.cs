using Common.Extensions;
using Common.Interfaces;
using LibraryAmoCRM.Infarstructure.QueryParams;
using LibraryAmoCRM.Interfaces;
using LibraryAmoCRM.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Common.BusinessLogicHelpers.Crm.Actions
{
    public class FindContactActions : ActionsBase
    {
        public FindContactActions(IDataManager amoManager, ILogger logger) 
            : base (amoManager, logger)
        {}

        public async Task<ContactDTO> LookForContact(int id)
        {
            if (id == null || id == 0) return null;

            var query = crm.Contacts.Get().Filter(p => p.Id = id);
            var result = await query.Execute();

            return result?.FirstOrDefault();
        }

        public async Task<ContactDTO> LookForContact(string phone = "", string email = "", string guid ="")
        {
            var query = await FindContact(guid);
            if (query != null) return query;

            query = await FindContact( CutPhoneCode(phone.LeaveJustDigits()) );
            if (query != null) return query;

            query = await FindContact(email.ClearEmail());

            return query;
        }

        public async Task<ContactDTO> LookForContact(IEnumerable<string> phones, IEnumerable<string> emails, string guid)
        {
            var query = await FindContact(guid);
            if (query != null) return query;

            query = await FindContact(ClearPhones(phones));
            if (query != null) return query;

            query = await FindContact(ClearEmails(emails));

            return query;

            IEnumerable<string> ClearPhones(IEnumerable<string> values)
            {
                var array = values.ToList();
                for (var i = 0; i < array.Count(); i++) { array[i] = CutPhoneCode( array[i].LeaveJustDigits() ); }
                return array;
            }
            IEnumerable<string> ClearEmails(IEnumerable<string> values)
            {
                var array = values.ToList();
                for (var i = 0; i < array.Count(); i++) { array[i] = array[i].ClearEmail(); }
                return array;
            }
        }


        public async Task<ContactDTO> FindContact(IEnumerable<string> queryParams)
        {
            if (queryParams == null || queryParams.Count() ==0 ) return null;

            var query = crm.Contacts.Get();

            foreach (var param in queryParams)
            {
                query.Filter(i => i.Query = param);
            }

            var result = await query.Execute();

            return result?.FirstOrDefault();
        }

        public async Task<ContactDTO> FindContact(string queryParam)
        {
            if (String.IsNullOrEmpty(queryParam)) return null;

            var query = crm.Contacts.Get().Filter(p => p.Query = queryParam);
            var result = await query.Execute();

            return result?.FirstOrDefault();
        }

        protected string CutPhoneCode(string value)
        {
            Regex regex = new Regex(@"[^0-9]");
            string str = regex.Replace(value.ToString(), "");

            string substring = str.Length >= 10 ? str.Substring(str.Length - 10) : str;

            return substring;
        }
    }
}
