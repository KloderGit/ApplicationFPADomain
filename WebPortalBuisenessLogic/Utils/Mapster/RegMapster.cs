using Common.Extensions.Models.Crm;
using Domain.Models.Crm;
using Domain.Models.Education;
using LibraryAmoCRM.Configuration;
using LibraryAmoCRM.Models;
using LibraryAmoCRM.Models.Fields;
using Mapster;
using ServiceLibraryNeoClient.Models;
using ServiceReference1C;
using System;
using System.Collections.Generic;
using System.Linq;
using WebPortalBuisenessLogic.Models.Crm;
using WebPortalBuisenessLogic.Models.DataBase;

namespace WebPortalBuisenessLogic.Utils.Mapster
{
    public class RegMapster
    {
        public RegMapster(TypeAdapterConfig mapper)
        {
            mapper.NewConfig<ProgramNode, EducationProgram>();

            mapper.NewConfig<FormNode, EducationForm>();

            mapper.NewConfig<DepartmentNode, Department>();

            mapper.NewConfig<SubjectNode, Subject>();

            mapper.NewConfig<ProgramNode, ProgramDTO>()
                .Map(dest => dest.Form, src => src.Form.Title)
                .Map(dest => dest.Department, src => src.Department.Title)
                .Map(dest => dest.Subjects, src => src.Subjects.Select(i => i.Title));

            // ----------------------------

            mapper.NewConfig<ProgramEdu, EducationProgram>()
                .Map(dest => dest.Guid, src => src.XML_ID)
                .Map(dest => dest.Title, src => src.name)
                .Map(dest => dest.Active, src => src.active == "Активный" ? true : false)
                .Map(dest => dest.Accepted, src => src.acceptDate)
                .Map(dest => dest.Type, src => src.typeProgram)
                .Map(dest => dest.Variant, src => src.viewProgram)
                .Map(dest => dest.Department, src => src.category)
                .Map(dest => dest.EducationForm, src => src.formEducation)
                .Map(dest => dest.Subjects, src => src.listOfSubjects);


            mapper.NewConfig<formEdu, EducationForm>()
                .Map(dest => dest.Guid, src => src.GUIDFormEducation)
                .Map(dest => dest.Title, src => src.Name);


            mapper.NewConfig<ГруппаПрограммыОбучения, Department>()
                .Map(dest => dest.Guid, src => src.ГУИД)
                .Map(dest => dest.Title, src => src.Наименование);

            mapper.NewConfig<category, Department>()
                .Map(dest => dest.Guid, src => src.GUID)
                .Map(dest => dest.Title, src => src.Name);

            mapper.NewConfig<subject, Subject>()
                .Map(dest => dest.Guid, src => src.GUIDsubject)
                .Map(dest => dest.Title, src => src.subjectName)
                .Map(dest => dest.Duration, src => src.duration)
                .Map(dest => dest.Attestation, src => String.IsNullOrEmpty(src.Attestation.formControl.GUIDFormControl) ? null : src.Attestation.formControl);


            mapper.NewConfig<ФормаКонтроля, Domain.Models.Education.Attestation>()
                .Map(dest => dest.Guid, src => src.ГУИД)
                .Map(dest => dest.Title, src => src.Наименование);

            mapper.NewConfig<formControl, Domain.Models.Education.Attestation>()
                .Map(dest => dest.Guid, src => src.GUIDFormControl)
                .Map(dest => dest.Title, src => src.Name);

            // --------------------------------------



            mapper.NewConfig<Domain.Models.Education.Attestation, AttestationNode>();

            mapper.NewConfig<Domain.Models.Education.Subject, SubjectNode>();

            mapper.NewConfig<Domain.Models.Education.Department, DepartmentNode>();

            mapper.NewConfig<Domain.Models.Education.EducationForm, FormNode>();

            mapper.NewConfig<Domain.Models.Education.EducationProgram, ProgramNode>()
                                .Map(dest => dest.Form, src => src.EducationForm);


            // --------------------------------------


            mapper.NewConfig<WizardDTO, Lead>()
                .IgnoreNullValues(true)
                .Map(dest => dest.Id, src => src.LeadId);

            mapper.NewConfig<WizardDTO, Contact>()
                .IgnoreNullValues(true)
                .Map(dest => dest.Id, src => src.ContactId);

            mapper.NewConfig<Lead, WizardDTO>()
                .IgnoreNullValues(true)
                .Map(dest => dest.LeadId, src => src.Id)
                .Map(dest => dest.Program, src => src.Program() != null ? src.Program().Enum: null)

                .Map(dest => dest.Birthday, src => src.MainContact.Birthday())
                .Map(dest => dest.City, src => src.MainContact.City())
                .Map(dest => dest.ContactId, src => src.MainContact.Id)
                .Map(dest => dest.Education, src => src.MainContact.Education())

                  .IgnoreIf((src, dest) => src.MainContact.Email() == null, dest => dest.Email)
                  .Map(dest => dest.Email, src => src.MainContact.Email().FirstOrDefault().Value)

                .Map(dest => dest.Expirience, src => src.MainContact.Experience())
                .Map(dest => dest.Name, src => src.MainContact.Name)

                  .IgnoreIf((src, dest) => src.MainContact.Phones() == null, dest => dest.Phone)
                  .Map(dest => dest.Phone, src => src.MainContact.Phones().FirstOrDefault().Value)

                .Map(dest => dest.ProgramPart, src => src.MainContact.GroupPart())
                .Map(dest => dest.Subway, src => src.MainContact.Location());


            mapper.NewConfig<Lead, LeadDTO>()
                .Map(dest => dest.Company, src => src.Company != null ? new CompanyField { Id = src.Company.Id, Name = src.Company.Name } : null)
                .Map(dest => dest.Contacts, src => src.Contacts != null ? src.Contacts : null)
                .Map(dest => dest.MainContact, src => src.MainContact != null ? new MainContactField { Id = src.MainContact.Id  } : null)
                .Map(dest => dest.CustomFields, src => src.Fields != null ? src.Fields : null)
            ;

            mapper.NewConfig<List<Contact>, ContactsField>()
                .Map(dest => dest.IDs, src => src != null ? src.Select(i => i.Id) : null);

        }

        public object src { get; }
    }
}
