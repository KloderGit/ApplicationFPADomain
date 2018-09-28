using Common.Extensions;
using Common.Extensions.Models.Crm;
using Common.Logging;
using Domain.Models.Crm;
using Library1C;
using Serilog;
using ServiceReference1C;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace WebApiBusinessLogic.Infrastructure.CrmDoEventActions.Shared
{
    public class LookForContactIn1C
    {
        UnitOfWork database;
        ILogger logger;

        string guid;

        public LookForContactIn1C(UnitOfWork database, ILogger logger)
        {
            this.database = database;
            this.logger = logger;
        }

        public async Task<string> Find(Contact contact)
        {
            if(contact.Phones() != null) await FindByPhones(contact.Phones().Select(x=>x.Value));

            if(contact.Email() != null) await FindByEmails(contact.Email().Select(x => x.Value));

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
                query = await database.Persons.GetGuidByPhoneOrEmail( phone, email);
            }
            catch (Exception ex)
            {
                var info = new MessageLocation(this)
                {
                    Metod = MethodBase.GetCurrentMethod().Name
                };

                logger.Error(ex, "Ошибка по адресу - {@Location}", info);
            }

            return query?.GUID;
        }
    }
}
