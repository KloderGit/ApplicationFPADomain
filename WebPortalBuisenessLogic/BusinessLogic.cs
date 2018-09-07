using Domain.Models.Crm;
using Domain.Models.Crm.Fields;
using Domain.Models.Education;
using Library1C;
using LibraryAmoCRM;
using LibraryAmoCRM.Configuration;
using LibraryAmoCRM.Infarstructure.QueryParams;
using LibraryAmoCRM.Models;
using Mapster;
using Microsoft.Extensions.Configuration;
using Serilog;
using ServiceLibraryNeoClient.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebPortalBuisenessLogic.Infrastructure.Extended;
using WebPortalBuisenessLogic.Models.Crm;
using WebPortalBuisenessLogic.Models.DataBase;
using WebPortalBuisenessLogic.Utils.Mapster;

namespace WebPortalBuisenessLogic
{
    public class BusinessLogic
    {
        ILogger logger;
        TypeAdapterConfig mapper;

        Lazy<DataManager> amocrm;
        Lazy<UnitOfWork> database;

        public BusinessLogic(ILogger logger, IConfiguration configuration, TypeAdapterConfig mapping)
        {
            this.logger = logger;
            this.mapper = mapping; new RegMapster(mapper);

            var amoAccount = configuration.GetSection("providers:0:AmoCRM:connection:account:name").Value;
            var amoUser = configuration.GetSection("providers:0:AmoCRM:connection:account:email").Value;
            var amoPass = configuration.GetSection("providers:0:AmoCRM:connection:account:hash").Value;

            var user1C = configuration.GetSection("providers:1:1C:connection:account:user").Value;
            var pass1C = configuration.GetSection("providers:1:1C:connection:account:pass").Value;

            this.amocrm = new Lazy<DataManager>(() => new DataManager(amoAccount, amoUser, amoPass));

            this.database = new Lazy<UnitOfWork>(new UnitOfWork(user1C, pass1C));
        }



        public async Task<bool> UpdateContact(UpdateFormDTO model)
        {
            bool result = false;

            if (model.ContactId == 0)
            {
                result = await AddFromForm(model);
                return result ? true : false;
            }

            try
            {
                var requesContacts = amocrm.Value.Contacts.Get().SetParam(x => x.Id = model.ContactId).Execute().Result;
                var contact = requesContacts.FirstOrDefault().Adapt<Contact>(mapper);

                var enumPhoneTypes = Enum.GetValues(typeof(PhoneType)).Cast<PhoneType>().Select(i => (int)i);
                var phoneTypes = enumPhoneTypes.Except(contact.GetField((int)ContactFieldsEnum.Phone).Select(e => e.Enum.Value));

                contact.SetField((int)ContactFieldsEnum.Phone, model.Phone, phoneTypes.FirstOrDefault());

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            return result;
        }














        public async Task<bool> UpdateFromForm(UpdateFormDTO model)
        {
            var d = UpdateContact(model);

            try
            {
                var contact = PrepareContact(model);


                await amocrm.Value.Contacts.Update(contact);

                var lead = new LeadDTO
                {
                    Id = model.LeadId,
                    CustomFields = new List<CustomField> {
                    new CustomField{
                        Id = 227457,
                        Values = new[] {
                            new CustomFieldValue{ Value = model.Program.ToString() }
                        }
                    }
                }
                };

                await amocrm.Value.Leads.Update(lead);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> AddFromForm(UpdateFormDTO model)
        {
            try
            {
                var contact = PrepareContact(model);
                contact.ResponsibleUserId = 2079676;

                var amoContact = !String.IsNullOrEmpty(model.Phone) ? findByPhone(model.Phone) : findByEmail(model.Email);

                amoContact = amoContact ?? amocrm.Value.Contacts.Add(contact).Result;

                var lead = new LeadDTO
                {
                    Name = "Сделка с планшета - Тест",
                    ResponsibleUserId = 2079676,
                    Status = 17793889,
                    CustomFields = new List<CustomField> {
                    new CustomField{
                        Id = 227457,
                        Values = new[] {
                            new CustomFieldValue{ Value = model.Program.ToString() }
                        }
                    }
                }
                };

                lead = await amocrm.Value.Leads.Add(lead);

                lead.Contacts = new ContactsField { IDs = new List<int> { amoContact.Id.Value } };

                await amocrm.Value.Leads.Update(lead);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            

            ContactDTO findByPhone(string phone)
            {
                var query = amocrm.Value.Contacts.Get().SetParam(x => x.Phone = phone).Execute().Result;
                return query?.FirstOrDefault();
            }

            ContactDTO findByEmail(string email)
            {
                var query = amocrm.Value.Contacts.Get().SetParam(x => x.Query = email).Execute().Result;
                return query?.FirstOrDefault();
            }
        }


        public async Task<IEnumerable<UpdateFormDTO>> GetPreparedLeads()
        {
            return await GetLeadsByStatus(17793889);
        }

        public async Task<IEnumerable<UpdateFormDTO>> GetLeadsByStatus(int status)
        {
            IEnumerable<UpdateFormDTO> result = new List<UpdateFormDTO>();

            try
            {
                var queryLeads = amocrm.Value.Leads.Get().SetParam(x => x.Status = status).Execute().Result;
                var leads = queryLeads.Adapt<IEnumerable<Lead>>(mapper);

                var ld = leads?.Where(cn => cn.Contacts != null || cn.MainContact != null).ToList();

                for (int xx = 0; xx < ld.Count(); xx++)
                {
                    ld[xx].MainContact = await GetContactById(ld[xx].MainContact.Id);
                }

                result = ld.Adapt<IEnumerable<UpdateFormDTO>>(mapper);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return result;
        }

        public async Task<Contact> GetContactById(int id)
        {
            var contacts = await amocrm.Value.Contacts.Get().SetParam(x => x.Id = id).Execute();
            return contacts.FirstOrDefault().Adapt<Contact>(mapper);
        }

        public async Task<UpdateFormDTO> GetLeadById(int id)
        {
            var queryLead = await amocrm.Value.Leads.Get().SetParam(x => x.Id = id).Execute();
            var lead = queryLead.FirstOrDefault().Adapt<Lead>(mapper);

            lead.MainContact = await GetContactById(lead.MainContact.Id);

            return lead.Adapt<UpdateFormDTO>(mapper);
        }


        public IEnumerable<ProgramDTO> GetDataBasePrograms()
        {
            var db = new ServiceLibraryNeoClient.Implements.DataManager();

            var progs = db.Programs.GetList();

            var result = progs.Adapt<IEnumerable<ProgramDTO>>(mapper);

            return result;
        }

        public async Task<bool> UpdateDataBasePrograms()
        {
            var db = new ServiceLibraryNeoClient.Implements.DataManager();

            var service1C = new Library1C.UnitOfWork("Kloder", "Kaligula2");

            var request = await service1C.Programs.GetList();

            var programs = request.Adapt<IEnumerable<EducationProgram>>(mapper);

            var ddd = programs.Adapt<IEnumerable<ProgramNode>>(mapper);


            foreach (var item in programs.Where(p => p.Active))
            {
                var m = item.Adapt<ProgramNode>(mapper);
                db.Programs.Add(m);
            }

            return true;
        }



        private ContactDTO PrepareContact(UpdateFormDTO model)
        {
            var contact = new ContactDTO
            {
                Name = model.Name,
                CustomFields = new List<CustomField>()
            };

            if (model.ContactId != 0)
            {
                contact.Id = model.ContactId;
            }

            if (!String.IsNullOrEmpty(model.Phone))
            {
                (contact.CustomFields as List<CustomField>).Add(
                    new CustomField
                    {
                        Id = 54667,
                        Values = new[] {
                            new CustomFieldValue{ Value = model.Phone, Enum = 114611 }
                        }
                    }
                );
            }

            if (!String.IsNullOrEmpty(model.Email))
            {
                (contact.CustomFields as List<CustomField>).Add(
                    new CustomField
                    {
                        Id = 54669,
                        Values = new[] {
                                        new CustomFieldValue{ Value = model.Email, Enum = 114621 }
                                    }
                    }
                );
            }

            if (model.Birthday != DateTime.MinValue)
            {
                (contact.CustomFields as List<CustomField>).Add(
                     new CustomField
                     {
                         Id = 565515,
                         Values = new[] {
                                        new CustomFieldValue{ Value = model.Birthday.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture), Enum = 114621 }
                                    }
                     }
                );
            }

            if (!String.IsNullOrEmpty(model.City))
            {
                (contact.CustomFields as List<CustomField>).Add(
                    new CustomField
                    {
                        Id = 72337,
                        Values = new[] {
                                        new CustomFieldValue{ Value = model.City }
                                    }
                    }
                );
            }

            if (!String.IsNullOrEmpty(model.Subway))
            {
                (contact.CustomFields as List<CustomField>).Add(
                    new CustomField
                    {
                        Id = 565525,
                        Values = new[] {
                                        new CustomFieldValue{ Value = model.Subway }
                                    }
                    }
                );
            }

            if (!String.IsNullOrEmpty(model.Expirience))
            {
                (contact.CustomFields as List<CustomField>).Add(
                        new CustomField
                        {
                            Id = 565519,
                            Values = new[] {
                                            new CustomFieldValue{ Value = model.Expirience }
                                        }
                        }
                );
            }

            if (!String.IsNullOrEmpty(model.Education))
            {
                (contact.CustomFields as List<CustomField>).Add(
                    new CustomField
                    {
                        Id = 565517,
                        Values = new[] {
                                        new CustomFieldValue{ Value = model.Education }
                                    }
                    }
                );
            }

            if (model.ProgramPart != 0)
            {
                (contact.CustomFields as List<CustomField>).Add(
                    new CustomField
                    {
                        Id = 565521,
                        Values = new[] {
                                        new CustomFieldValue{ Value = model.ProgramPart.ToString() }
                                    }
                    }
                );
            }

            return contact;
        }
    }
}