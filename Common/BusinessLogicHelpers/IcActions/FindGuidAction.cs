using Common.Extensions;
using Common.Extensions.Models.Crm;
using Domain.Models.Crm;
using Library1C;
using Microsoft.Extensions.Logging;
using ServiceReference1C;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace Common.BusinessLogicHelpers.IcActions
{
    public class FindGuidAction
    {
        UnitOfWork database;
        ILoggerFactory loggerFactory;
        ILogger currentLogger;

        string guid;

        public FindGuidAction(UnitOfWork database, ILoggerFactory loggerFactory)
        {
            this.database = database;

            this.loggerFactory = loggerFactory;
            this.currentLogger = loggerFactory.CreateLogger(this.ToString());
        }

        public async Task<string> Find(Contact contact)
        {
            if (contact.Phones() != null) await FindByPhones(contact.Phones().Select(x => x.Value));

            if (contact.Email() != null) await FindByEmails(contact.Email().Select(x => x.Value));

            return guid;
        }

        private async Task FindByPhones(IEnumerable<string> phones)
        {
            foreach (var phone in phones)
            {
                if (String.IsNullOrEmpty(guid))
                {
                    guid = await GetGuid(phone.Length >= 10 ? phone.Substring(phone.Length - 10) : phone, "");
                }
            }
        }
        private async Task FindByEmails(IEnumerable<string> emails)
        {
            foreach (var email in emails)
            {
                if (String.IsNullOrEmpty(guid))
                {
                    guid = await GetGuid("", email.ClearEmail());
                }
            }
        }

        private async Task<string> GetGuid(string phone, string email)
        {
            flGUIDs query = null;

            try
            {
                query = await database.Persons.GetGuidByPhoneOrEmail(phone, email);
            }
            catch (Exception ex)
            {
                currentLogger.LogError(ex, "Ошибка по адресу GUID в 1С полям {@phone} и {@email}", phone, email);
            }

            return query?.GUID;
        }
    }
}
