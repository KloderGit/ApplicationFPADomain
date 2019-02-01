using Common.BusinessLogicHelpers.Crm.Actions;
using Common.Extensions.Models.Crm;
using Common.Interfaces;
using Common.Mapping;
using Domain.Models.Crm;
using LibraryAmoCRM.Configuration;
using LibraryAmoCRM.Interfaces;
using LibraryAmoCRM.Models;
using Mapster;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiBusinessLogic.Logics.SignUp.Fabric;
using WebApiBusinessLogic.Logics.SignUp.Model;

namespace WebApiBusinessLogic.Logics.SignUp
{
    public class SignUpLogic
    {
        ILogger currentLogger;
        ILoggerFactory loggerFactory;

        TypeAdapterConfig mapper;
        IDataManager crm;

        public SignUpLogic(TypeAdapterConfig mapping, IDataManager amocrm, ILoggerFactory loggerFactory)
        {
            // Логи
            this.currentLogger = loggerFactory.CreateLogger(this.ToString());
            this.loggerFactory = loggerFactory;

            this.mapper = mapping;  // Maps
                new RegisterCommonMaps( mapper );
            this.crm = amocrm; // Amo
        }
        
        public async Task<int> AddLead(SignUpDTO model)
        {
            // Build Lead
            var buildLead = new LeadBuilder();
                buildLead.Name(model.Lead.EducationType, GetInterestingEvent(), model.Lead.Name);
                buildLead.Price( model.Lead.Price );
                buildLead.Guid( model.Lead.Guid );
                buildLead.Date( model.Lead.Date );
                buildLead.EducationType( model.Lead.EducationForm );
            var lead = (Lead)buildLead;

            try
            {
                System.Threading.Tasks.Task taskPrepareLeadLog = System.Threading.Tasks.Task.Factory.StartNew(
                   () => currentLogger.LogInformation("Подготовлена сделка для добавления - {Lead}", lead)
                );

                var query = await crm.Leads.Add(lead.Adapt<LeadDTO>(mapper));
                lead = query.Adapt<Lead>( mapper );

                //System.Threading.Tasks.Task taskCreateLeadLog = System.Threading.Tasks.Task.Factory.StartNew(
                //   () => logger.Information( GetType().Assembly.GetName().Name + " | Создана сделка с сайта Lead Id - {Lead}", lead.Id )
                //);                
                System.Threading.Tasks.Task taskCreateLeadLog = System.Threading.Tasks.Task.Factory.StartNew(
                   () => currentLogger.LogInformation("Создана сделка с сайта Lead Id - {Lead}", lead.Id)
                );
            }
            catch(Exception ex)
            {
                currentLogger.LogWarning( ex, "Ошибка создания сделки с сайта" );
                throw new Exception();
            }


            // Add Lead's Notes
            var systemNote = new NoteDTO()
            {
                ElementId = lead.Id,
                ElementType = (int)ElementTypeEnum.Сделка,
                NoteType = 25,
                Params = new NoteParams
                {
                    Text = "Адрес отправки запроса: Сайт \\ Лендинг",
                    Service = "WebApi | "
                }
            };

            var modelNote = new NoteDTO()
            {
                ElementId = lead.Id,
                ElementType = (int)ElementTypeEnum.Сделка,
                NoteType = 4,
                Text = "Заявка с сайта \r\n"
                    + "ФИО - " + model.Contact.Name + "\r\n" +
                    "Тел.: - " + model.Contact.Phone + "\r\n" +
                    "Email: - " + model.Contact.Email + "\r\n" +
                    "Город - " + model.Contact.City + "\r\n" +
                    "Мероприятие - " + model.Lead.Name + "\r\n" +
                    "Дата - " + model.Lead.Date.ToString("dd-MM-yyyy") + "\r\n"
            };

            try
            {
                System.Threading.Tasks.Task taskAddNote = System.Threading.Tasks.Task.Factory.StartNew(
                   () => {
                       crm.Notes.Add( new [] { systemNote, modelNote } );
                   }
                );
            }
            catch (Exception ex)
            {
                currentLogger.LogWarning(ex, "Ошибка создания примечания для сделки - " + lead.Id);
            }


            // Contact for Lead
            System.Threading.Tasks.Task taskCreateContact = System.Threading.Tasks.Task.Factory.StartNew(
               async () =>
               {
                   var contact = await FindOrCreateContact(model);

                   await LinkContactToLead(lead.Id, contact.Id);
               }
            );

            return lead.Id;
        }

        void GivenFromLending()
        { }



        protected async Task<Contact> FindOrCreateContact(SignUpDTO model)
        {
            // Check available contact
            var foundContact = await new FindContactActions(crm, currentLogger)
                .LookForContact(model.Contact.Phone, model.Contact.Email, null);

            var contact = foundContact != null ? foundContact.Adapt<Contact>(mapper) : new Contact();

            // Create contact
            if (foundContact == null)
            {
                var buildContact = new Common.BusinessLogicHelpers.Crm.Builders.ContactBuilder();
                buildContact.Name(model.Contact.Name);
                buildContact.Phone(model.Contact.Phone);
                buildContact.Email(model.Contact.Email);
                buildContact.City(model.Contact.City);
                contact = (Contact)buildContact;

                var contactDTO = contact.Adapt<ContactDTO>(mapper);

                try
                {
                    System.Threading.Tasks.Task taskPrepareContactLog = System.Threading.Tasks.Task.Factory.StartNew(
                       () => currentLogger.LogInformation("Подготовлен контакт для добавления - {@Contact}", contact)
                    );

                    var query = await crm.Contacts.Add(contactDTO);
                    contact = query.Adapt<Contact>(mapper);

                    System.Threading.Tasks.Task taskreateContactLog = System.Threading.Tasks.Task.Factory.StartNew(
                        () => currentLogger.LogInformation("Создан контакт {Name} | {Id}", contactDTO.Name, contact.Id)
                    );
                }
                catch (Exception ex)
                {
                    currentLogger.LogWarning(ex, "Ошибка создания контакта с сайта");
                    throw new Exception();
                }
            }

            return contact;
        }

        protected async System.Threading.Tasks.Task LinkContactToLead(int leadId, int contactId)
        {
            var updateLead = new Lead();
            updateLead.Id = leadId;
            updateLead.Contacts = new List<Contact> { new Contact { Id = contactId } };

            try
            {
                await crm.Leads.Update(updateLead.Adapt<LeadDTO>(mapper));

                System.Threading.Tasks.Task taskCreateLeadLog = System.Threading.Tasks.Task.Factory.StartNew(
                        () => currentLogger.LogInformation("К сделке - {Lead} прикреплён клиент - {Contact}", leadId, contactId)
                );
            }
            catch (Exception ex)
            {
                currentLogger.LogWarning(ex, "Ошибка добавления пользователя в сделку");
                throw new Exception();
            }
        }

        protected Dictionary<int, string> GetInterestingEvent()
        {
            if (crm.Account == null) return new Dictionary<int, string>();

            var events = crm.Account.Embedded.CustomFields.Leads.FirstOrDefault(x => x.Key == 66349).Value.Enums;
            var programs = crm.Account.Embedded.CustomFields.Leads.FirstOrDefault(x => x.Key == 227457).Value.Enums;

            return events.Union(programs).ToDictionary(pair => pair.Key, pair => pair.Value);
        }
        
    }
}
