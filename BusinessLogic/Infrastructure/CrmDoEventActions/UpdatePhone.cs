using Common.Configuration.Crm;
using Common.Logging;
using Library1C;
using LibraryAmoCRM;
using LibraryAmoCRM.Infarstructure.QueryParams;
using LibraryAmoCRM.Models;
using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
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

            var toUpdate = false;

            try
            {
                var amoUser = amoManager.Contacts.Get().SetParam(prm => prm.Id = int.Parse(e.EntityId))
                                .Execute()
                                    .Result
                                    .FirstOrDefault();

                phones = amoUser.CustomFields.Where(f => f.Id == (int)ContactFieldsEnum.Phone).FirstOrDefault();

                foreach(var phone in phones.Values)
                {
                    if ( !(phone.Value == ClearPhone(phone.Value)))
                    {
                        phone.Value = ClearPhone(phone.Value);
                        toUpdate = true;
                    }                    
                }

                if ( toUpdate ) await amoManager.Contacts.Update(amoUser);

            }
            catch (Exception ex)
            {
                var info = new MessageLocation(this);
                info.Metod = MethodBase.GetCurrentMethod().Name;

                //logger.Error("Ошибка, {@Location}", info);
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
