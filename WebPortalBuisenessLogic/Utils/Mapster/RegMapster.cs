using Domain.Models.Crm;
using Domain.Models.Education;
using LibraryAmoCRM.Models;
using Mapster;
using ServiceLibraryNeoClient.Models;
using ServiceReference1C;
using System;
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

            mapper.NewConfig<Lead, UpdateFormDTO>()
              .IgnoreNullValues(true)
              .Map(dest => dest.LeadId, src => src.Id)
              .Map(dest => dest.ContactId, src => src.MainContact.Id)
              .Map(dest => dest.Name, src => src.MainContact != null ? src.MainContact.Name : null)
              .Map(dest => dest.Phone, src => src.MainContact.Fields.FirstOrDefault(pr => pr.Id == 54667) != null ?
                                                    src.MainContact.Fields.FirstOrDefault(pr => pr.Id == 54667).Values.FirstOrDefault().Value : null)
              .Map(dest => dest.Email, src => src.MainContact.Fields.FirstOrDefault(pr => pr.Id == 54669) != null ?
                                                    src.MainContact.Fields.FirstOrDefault(pr => pr.Id == 54669).Values.FirstOrDefault().Value : null);

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




        }
    }
}
