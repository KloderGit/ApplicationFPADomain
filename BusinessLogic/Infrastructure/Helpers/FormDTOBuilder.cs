using Common.Extensions.Models.Crm;
using Domain.Models.Crm;
using LibraryAmoCRM.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebApiBusinessLogic.Infrastructure.Helpers
{
    public class FormDTOBuilder
    {
        private Contact contact;
        private Lead lead;

        public FormDTOBuilder()
        {
            contact = new Contact();
            lead = new Lead();
        }

        public FormDTOBuilder(Contact contact)
        {
            this.contact = contact;
            lead = new Lead();
            this.lead.Sources(139517);
        }

        public FormDTOBuilder(Lead lead)
        {
            contact = new Contact();
            this.lead = lead;
        }

        public FormDTOBuilder(Contact contact, Lead lead)
        {
            this.contact = contact ?? new Contact();
            this.lead = lead ?? new Lead();

            this.lead.Sources(139517);
        }


        public FormDTOBuilder EducationType(string value)
        {
            if (String.IsNullOrEmpty(value)) value = "";

            EducationTypeEnum manager;
            PipelineStartStatusEnum status;

            try
            {
                manager = (EducationTypeEnum)Enum.Parse(typeof(EducationTypeEnum), value.ToUpper().Trim());
                status = (PipelineStartStatusEnum)Enum.Parse(typeof(PipelineStartStatusEnum), value.ToUpper().Trim());
            }
            catch (ArgumentException ex)
            {
                manager = EducationTypeEnum.Default;
                status = PipelineStartStatusEnum.Default;
            }
            catch (NullReferenceException ex)
            {
                manager = EducationTypeEnum.Default;
                status = PipelineStartStatusEnum.Default;
            }

            contact.ResponsibleUserId = (int)manager;

            lead.ResponsibleUserId = (int)manager;
            lead.Status = (int)status;

            return this;
        }

        public FormDTOBuilder ContactName(string value)
        {
            if (String.IsNullOrEmpty(value)) return this;

            string name = value;

            if (!String.IsNullOrEmpty(this.contact.Name))
            {
                if (this.contact.Name.Length > value.Length) name = this.contact.Name;
            }

            this.contact.Name = name;

            return this;
        }

        public FormDTOBuilder LeadName(Dictionary<int, string> leadEvents, string type, string value)
        {
            string name = value;

            if (!String.IsNullOrEmpty(this.lead.Name))
            {
                if (this.lead.Name.Length > value.Length) name = this.lead.Name;
            }

            this.lead.Name = "[ TEST ] " + name;

            var leadEvent = leadEvents.FirstOrDefault(i => i.Value.ToUpper().Trim() == value.ToUpper().Trim());

            if (!leadEvent.Equals(default(KeyValuePair<int, string>)))
            {
                if (type == "Семинары")
                {
                    this.lead.Seminar(leadEvent.Key);
                }
                if (type == "Курсы")
                {
                    this.lead.Program(leadEvent.Key);
                }
                this.lead.Fields = this.lead.Fields ?? new List<Domain.Models.Crm.Fields.Field>();
                this.lead.Fields = this.lead.GetChanges().Fields;
            }

            return this;
        }

        public FormDTOBuilder City(string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                this.contact.City(value);
                this.contact.Fields = this.contact.Fields ?? new List<Domain.Models.Crm.Fields.Field>();
                this.contact.Fields = this.contact.GetChanges().Fields;
            }
                
            return this;
        }

        public FormDTOBuilder Price(int value)
        {
            if (value != 0 && this.lead.Price != value) this.lead.Price = value;
            return this;
        }

        public FormDTOBuilder DateOfEvent(DateTime? value)
        {
            if (value != null)
            {
                this.lead.SeminarDate(value.Value);
                this.lead.Fields = this.lead.Fields ?? new List<Domain.Models.Crm.Fields.Field>();
                this.lead.Fields = this.lead.GetChanges().Fields;
            }

            return this;
        }

        public FormDTOBuilder Phone(IEnumerable<string> value)
        {
            if (value == null || value.Count() == 0) return this;

            contact.Phones(PhoneTypeEnum.MOB, value.FirstOrDefault());
            this.contact.Fields = this.contact.Fields ?? new List<Domain.Models.Crm.Fields.Field>();
            this.contact.Fields = this.contact.GetChanges().Fields;

            return this;
        }


        public FormDTOBuilder Email(IEnumerable<string> value)
        {
            if (value == null || value.Count() == 0) return this;

            contact.Email(EmailTypeEnum.PRIV, value.FirstOrDefault());

            this.contact.Fields = this.contact.Fields ?? new List<Domain.Models.Crm.Fields.Field>();
            this.contact.Fields = this.contact.GetChanges().Fields;

            return this;
        }

        public FormDTOBuilder LeadGuid(string value)
        {
            if (String.IsNullOrEmpty(value)) return this;

            this.lead.Guid(value);
            this.lead.Fields = this.lead.Fields ?? new List<Domain.Models.Crm.Fields.Field>();
            this.lead.Fields = this.lead.GetChanges().Fields;

            return this;
        }


        public static implicit operator Contact(FormDTOBuilder builder)
        {
            return builder.contact;
        }

        public static implicit operator Lead(FormDTOBuilder builder)
        {
            return builder.lead;
        }
    }

    enum EducationTypeEnum
    {
        Default = (int)ResponsibleUserEnum.Анастасия_Столовая,
        ОТКРЫТОЕ = (int)ResponsibleUserEnum.Ирина_Моисеева,
        ОТКРЫТАЯ = (int)ResponsibleUserEnum.Ирина_Моисеева,
        КОРПОРАТИВНОЕ = (int)ResponsibleUserEnum.Лина_Серрие,
        КОРПОРАТИВНАЯ = (int)ResponsibleUserEnum.Лина_Серрие,
        ОЧНОЕ = (int)ResponsibleUserEnum.Лина_Серрие,
        ОЧНАЯ = (int)ResponsibleUserEnum.Лина_Серрие,
        ДИСТАНЦИОННАЯ = (int)ResponsibleUserEnum.Ксения_Харымова,
    }

    enum PipelineStartStatusEnum
    {
        Default = 18664336,
        ОТКРЫТОЕ = 17769205,
        ОТКРЫТАЯ = 17769205,
        КОРПОРАТИВНОЕ = 17793877,
        ОЧНОЕ = 17793886,
        ДИСТАНЦИОННАЯ = 18855163,
    }

}
