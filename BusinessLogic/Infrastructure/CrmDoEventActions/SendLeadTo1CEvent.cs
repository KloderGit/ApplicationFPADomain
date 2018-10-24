using Common.DTO.Service1C;
using Common.Extensions.Models.Crm;
using Common.Interfaces;
using Domain.Models.Crm;
using Library1C;
using LibraryAmoCRM;
using LibraryAmoCRM.Configuration;
using LibraryAmoCRM.Infarstructure.QueryParams;
using LibraryAmoCRM.Interfaces;
using LibraryAmoCRM.Models;
using Mapster;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using WebApiBusinessLogic.Models.Crm;

namespace WebApiBusinessLogic.Infrastructure.CrmDoEventActions
{
    public class SendLeadTo1CEvent
    {
        IDataManager crm;
        UnitOfWork database;

        TypeAdapterConfig mapper;
        ILoggerService logger;

        public SendLeadTo1CEvent(IDataManager amocrm, UnitOfWork service1C, CrmEventTypes @Events, TypeAdapterConfig mapper, ILoggerService logger)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.crm = amocrm;
            this.database = service1C;

            Events.Status += DoAction;
        }

        public async void DoAction(object sender, CrmEvent e)
        {
            if (e.Entity != "leads" || String.IsNullOrEmpty(e.EntityId)) return;
            var @event = e.Events.FirstOrDefault();
            if (@event.Event != "status" ||
                (@event.CurrentValue != "19368232" & @event.CurrentValue != "142" & @event.CurrentValue != "18855166" )) return;

            Lead lead = null;
            Contact contact = null;

            var contactGuid = String.Empty;

            #region Получить сделку
            try
            {
                var queryLead = await crm.Leads.Get().Filter(i => i.Id = int.Parse(e.EntityId)).Execute();
                lead = queryLead.FirstOrDefault().Adapt<Lead>(mapper);
                if (lead == null) throw new NullReferenceException();
            }
            catch (NullReferenceException ex)
            {
                logger.Warning(ex, "Сделка ID - {ID} не найдена", e.EntityId);
                return;
            }
            catch (Exception ex)
            {
                logger.Debug(ex, "Ошибка при запросе Lead - {ID}", e.EntityId);
            }
            #endregion


            #region Получить контакт
            if (lead.MainContact != null)
            {
                try
                {
                    var queryContact = await crm.Contacts.Get().Filter(i => i.Id = lead.MainContact.Id).Execute();
                    contact = queryContact.FirstOrDefault().Adapt<Contact>(mapper);
                    if (contact == null) throw new NullReferenceException();
                }
                catch (NullReferenceException ex)
                {
                    logger.Warning(ex, "Контакт ID - {ID} не найден", lead.MainContact.Id);
                    return;
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Ошибка при запросе Contact - {ID}", lead.MainContact.Id);
                }
            }
            #endregion

            contactGuid = contact.Guid();

            #region Отправить пользователя в 1С если отсутствует

            if (String.IsNullOrEmpty(contact.Guid()))
            {
                var userActions = new Actions.User1C(database, logger, mapper);

                string guid = String.Empty;

                try
                {
                    guid = await userActions.Create(contact);
                    if (guid == null) throw new NullReferenceException();
                }
                catch (NullReferenceException ex)
                {
                    logger.Warning(ex, "Контакт ID - {ID} не создан в 1С", contact.Id);

                    var note = new NoteDTO()
                    {
                        ElementId = contact.Id,
                        ElementType = (int)ElementTypeEnum.Контакт,
                        NoteType = 25,
                        Params = new NoteParams
                        {
                            Text = "Ошибка сохранения пользователя в 1С.",
                            Service = "WebApi | "
                        }
                    };

                    var queryCreateTask = await crm.Notes.Add(note);

                    return;
                }
                catch (ArgumentException ex)
                {
                    var exceptionNote = new NoteDTO()
                    {
                        ElementId = contact.Id,
                        ElementType = (int)ElementTypeEnum.Контакт,
                        NoteType = 4,
                        Text = "Ошибка сохранения пользователя в 1С. Сообщение - " + ex.Message
                    };

                    var queryCreateTask = await crm.Notes.Add(exceptionNote);
                    return;
                }
                catch (Exception ex)
                {
                    logger.Debug(ex, "Ошибка при сохранении пользователя в 1С");
                    return;
                }

                contactGuid = guid;

                var noteSucces = new NoteDTO()
                {
                    ElementId = contact.Id,
                    ElementType = (int)ElementTypeEnum.Контакт,
                    NoteType = 25,
                    Params = new NoteParams
                    {
                        Text = "Контакт занесен в 1С",
                        Service = "WebApi | "
                    }
                };

                var queryCreateTaskSucces = await crm.Notes.Add(noteSucces);

                contact.Guid(guid);

                await crm.Contacts.Update(contact.GetChanges().Adapt<ContactDTO>(mapper));
            }
            #endregion


            var dto = new SendLeadto1CDTo();
            dto.ProgramGuid = lead.Guid();
            dto.UserGuid = contactGuid;
            dto.ContractPrice = lead.Price.Value;
            dto.ContractTitle = contact.Name;
            dto.DecreeTitle = contact.Name;

            if (lead.Pipeline.Id == 917056 || lead.Pipeline.Id == 920008 || lead.Pipeline.Id == 1102975)
            {
                dto.ContractEducationStart = lead.SeminarDate().Value;
                dto.ContractEducationEnd = lead.SeminarDate().Value.AddDays(3);
                dto.ContractExpire = lead.SeminarDate().Value.AddDays(3);
                dto.ContractGroup = lead.SeminarDate().Value.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
            }

            if (lead.Pipeline.Id == 920011 || lead.Pipeline.Id == 1042456)
            {
                dto.ContractEducationStart = lead.ProgramStartDate().Value;
                dto.ContractEducationEnd = lead.ProgramStartDate().Value.AddDays(180);
                dto.ContractExpire = lead.ContractExpireDate().Value;

                dto.ContractGroup = "Общая группа";

                if (lead.Pipeline.Id == 1042456) dto.ContractGroup = lead.DistantGroup().Value;
                if (lead.Pipeline.Id == 920011) dto.ContractGroup = lead.FullTimeGroup().Value;

                dto.ContractSubGroup = lead.SubGroup()?.Value ?? "";
            }

            var userActions3 = new Actions.User1C(database, logger, mapper);

            try
            {
                var result = await userActions3.SendLead( dto );

                var note = new NoteDTO()
                {
                    ElementId = lead.Id,
                    ElementType = (int)ElementTypeEnum.Сделка,
                    NoteType = 25,
                    Params = new NoteParams
                    {
                        Text = "Сделка успешно отправлена в 1С.",
                        Service = "WebApi | "
                    }
                };

                var queryCreateTask = await crm.Notes.Add( note );

                logger.Information( "Сделка успешно отправлена в 1С. Мероприятие - {Event} [{LeadId}] | Контакт - {Name} [{ContactId}]", lead.Name, lead.Id, contact.Name, contact.Id );

                lead.IsInService1C(true);

                await crm.Leads.Update( lead.GetChanges().Adapt<LeadDTO>( mapper ) );

            }
            catch (ArgumentException ex)
            {
                var exceptionNote = new NoteDTO()
                {
                    ElementId = lead.Id,
                    ElementType = (int)ElementTypeEnum.Сделка,
                    NoteType = 4,
                    Text = "Ошибка ошибка отправки сделки в 1С. Проверьте поля данных или зачислите вручную."
                };

                var queryCreateTask = await crm.Notes.Add( exceptionNote );

                logger.Warning( ex, "Ошибка отправки сделки в 1С" );
            }
            catch (Exception ex)
            {
                logger.Warning(ex, "Ошибка ошибка отправки сделки в 1С" );
            }
        }

    }
}
