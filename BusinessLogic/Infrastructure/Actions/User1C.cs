using Common.Extensions;
using Common.Extensions.Models.Crm;
using Common.Logging;
using Domain.Models.Crm;
using Library1C;
using Mapster;
using Serilog;
using ServiceReference1C;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WebApiBusinessLogic.Infrastructure.Actions
{
    public class User1C
    {
        ILogger logger;
        UnitOfWork database;

        public User1C(UnitOfWork database, ILogger logger)
        {
            this.logger = logger;
            this.database = database;
        }

        public async Task<string> Create(Contact contact)
        {
            string result = String.Empty;

            if (String.IsNullOrEmpty(contact.Name)) throw new ArgumentNullException("Отсутствует обязательное поля Name");

            var query = await database.Persons.Add( 
                FIO: contact.Name,
                Phone: contact.Phones()?.FirstOrDefault().Value.LeaveJustDigits(),
                Email: contact.Email()?.FirstOrDefault().Value.ClearEmail(),
                City: contact.City(),
                Address: contact.Location(),
                Education: contact.Education(),
                Expirience: contact.Experience(),
                Position: contact.Position(),
                BirthDay: DateTime.MinValue
            );

            if (query != null) result = query.GUID;

            return result;
        }

        public async Task<string> Find(Contact contact)
        {
            string guid = String.Empty;
            guid = await FindByPhones(contact.Phones().Select(x => x.Value));
            if(String.IsNullOrEmpty(guid)) guid = await FindByEmails(contact.Email().Select(x => x.Value));

            return guid;
        }

        private async Task<string> FindByPhones(IEnumerable<string> phones)
        {
            string guid = String.Empty;

            foreach (var phone in phones)
            {
                if (String.IsNullOrEmpty(guid))
                {
                    var chkPhone = phone.LeaveJustDigits();
                    guid = await GetGuid(chkPhone.Length >= 10 ? chkPhone.Substring(phone.Length - 10) : chkPhone, "");
                    if (!String.IsNullOrEmpty(guid)) return guid;
                }
            }
            return null;
        }

        private async Task<string> FindByEmails(IEnumerable<string> emails)
        {
            string guid = String.Empty;

            foreach (var email in emails)
            {
                if (String.IsNullOrEmpty(guid))
                {
                    guid = await GetGuid("", email.ClearEmail());
                    if (!String.IsNullOrEmpty(guid)) return guid;
                }
            }

            return null;
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
                var info = new MessageLocation(this)
                {
                    Metod = MethodBase.GetCurrentMethod().Name
                };

                logger.Error("Ошибка, {@Message}, По адресу - {@Location}", ex.Message, info);
            }

            return query?.GUID;
        }
    }
}
