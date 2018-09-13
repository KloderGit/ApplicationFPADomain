using Library1C;
using LibraryAmoCRM;
using LibraryAmoCRM.Infarstructure.QueryParams;
using LibraryAmoCRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApiBusinessLogic.Models.Crm;

namespace WebApiBusinessLogic.Infrastructure.CrmDoEventActions
{
    public class UpdateGuidByChangeLeadStatus : DoCrmActionBase
    {
        public UpdateGuidByChangeLeadStatus(DataManager amocrm, UnitOfWork database, CrmEventTypes @Events)
        : base(amocrm, database)
        {
            Events.Update += DoAction;
            Events.Add += DoAction;
        }

        public async override void DoAction(object sender, CrmEvent e)
        {
            if (e.Entity != "leads" || String.IsNullOrEmpty(e.EntityId)) return;

            LeadDTO lead = null;
            try
            {
                lead = amoManager.Leads.Get().SetParam(x => x.Id = int.Parse(e.EntityId)).Execute().Result.FirstOrDefault();
                if ( lead == null || lead.CustomFields.FirstOrDefault(x => x.Id == 570933) == null) return;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            List<ContactDTO> contacts = new List<ContactDTO>();

            try
            {
                var contactAll = new List<ContactDTO>();
                var contactsWithOutGuid = new List<ContactDTO>();                

                foreach (var i in lead.Contacts?.IDs)
                {
                    var contact = amoManager.Contacts.Get().SetParam(d => d.Id = i).Execute().Result.FirstOrDefault();
                    if (contact != null) { contactAll.Add(contact); }
                }

                contactsWithOutGuid = contactAll.Where(ip => ip.CustomFields.FirstOrDefault(i => i.Id == 571611) == null).ToList();
                contacts = contactAll.Except(contactsWithOutGuid).ToList();


                foreach (var cnt in contactsWithOutGuid)
                {

                }


                // Добавим в 1С тех кто в contactsWithOutGuid
                // Получим из 1С тнх кого создали
                // Добавим в contacts

                // Результат - Контакты и сделка имеют гуиды - Отправляем в 1С

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }
    }
}
