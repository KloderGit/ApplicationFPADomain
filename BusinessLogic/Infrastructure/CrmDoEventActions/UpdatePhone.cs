using Library1C;
using LibraryAmoCRM;
using LibraryAmoCRM.Configuration;
using LibraryAmoCRM.Infarstructure.QueryParams;
using LibraryAmoCRM.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using WebApiBusinessLogic.Infrastructure.CrmDoEventActions.Shared;
using WebApiBusinessLogic.Models.Crm;

namespace WebApiBusinessLogic.Infrastructure.CrmDoEventActions
{
    public class UpdatePhone : DoCrmActionBase
    {
        public UpdatePhone(DataManager amocrm, UnitOfWork database, CrmEventTypes @Events)
            :base (amocrm, database)
        {
            Events.Update += DoAction;
            Events.Add += DoAction;
        }

        public async override void DoAction(object sender, CrmEvent e)
        {
            if (e.Entity != "contacts" || String.IsNullOrEmpty(e.EntityId) || e.ContactType != "contact") return;

            var phones = new  CustomField();

            try
            {
                var amoUser = amoManager.Contacts.Get().SetParam(prm => prm.Id = int.Parse(e.EntityId))
                                .Execute()
                                    .Result
                                    .FirstOrDefault();

                phones = amoUser.CustomFields.Where(f => f.Id == (int)ContactFieldsEnum.Phone).FirstOrDefault();

                foreach(var phone in phones.Values)
                {
                    phone.Value = ClearPhone(phone.Value);
                }

                await amoManager.Contacts.Update(amoUser);

            }
            catch (Exception ex)
            {

            }

        }

        private string ClearPhone(string phone)
        {
            Regex rgx = new Regex(@"[^0-9]");
            var str = rgx.Replace(phone, "");

            return str;
        }
    }
}
