using Library1C;
using LibraryAmoCRM.Models;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebApiBusinessLogic.Infrastructure.CrmDoEventActions.Shared
{
    public class LookForContactGuid
    {
        UnitOfWork database;

        string guidResult = String.Empty;

        public LookForContactGuid(UnitOfWork database)
        {
            this.database = database;
        }

        public async Task<string> Find(ContactDTO amoUser)
        {
            var found = false;

            try
            {
                foreach (var field in amoUser.CustomFields.Where(v => v.Id == 54667))
                {
                    foreach (var phonefield in field.Values)
                    {
                        if (found != true)
                        {
                            var phone = phonefield.Value;

                            guidResult = await GetGuid(ClearPhone(phone), null);

                            found = String.IsNullOrEmpty(guidResult) ? false : true;
                        }
                    }
                }

                foreach (var field in amoUser.CustomFields.Where(v => v.Id == 54669))
                {
                    foreach (var emailfield in field.Values)
                    {
                        if (found != true)
                        {
                            var email = emailfield.Value;

                            guidResult = await GetGuid(null, ClearEmail(email));

                            found = String.IsNullOrEmpty(guidResult) ? false : true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return guidResult;
        }


        private async Task<string> GetGuid(string phone, string email)
        {
            var guid = await database.Persons.GetGuidByPhoneOrEmail(phone, email);

            return guid?.GUID;
        }

        private string ClearPhone(string phone)
        {
            Regex rgx = new Regex(@"[^0-9]");
            var str = rgx.Replace(phone, "");

            return str.Length >= 10 ? str.Substring(str.Length - 10) : str;
        }

        private string ClearEmail(string email)
        {
            Regex rgx = new Regex(@"[^a-zA-Z0-9\._\-@]");
            var str = rgx.Replace(email, "");

            return str;
        }
    }
}
