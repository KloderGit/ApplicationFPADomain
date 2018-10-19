using Common.DTO.Service1C;
using Common.Extensions;
using Common.Extensions.Models.Crm;
using Domain.Models.Crm;
using Domain.Models.Education;
using Mapster;
using ServiceReference1C;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Mapping
{
    public class Domain_1C
    {
        public Domain_1C(TypeAdapterConfig config)
        {
            config.NewConfig<ProgramEdu, EducationProgram>()
                .Map( dest => dest.Guid, src => src.XML_ID )
                .Map( dest => dest.Title, src => src.name )
                .Map( dest => dest.Active, src => src.active == "Активный" ? true : false )
                .Map( dest => dest.Accepted, src => src.acceptDate )
                .Map( dest => dest.Type, src => src.typeProgram )
                .Map( dest => dest.Variant, src => src.viewProgram )
                .Map( dest => dest.Department, src => src.category )
                .Map( dest => dest.EducationForm, src => src.formEducation )
                .Map( dest => dest.Subjects, src => src.listOfSubjects );


            config.NewConfig<formEdu, EducationForm>()
                .Map( dest => dest.Guid, src => src.GUIDFormEducation )
                .Map( dest => dest.Title, src => src.Name );


            config.NewConfig<ГруппаПрограммыОбучения, Department>()
                .Map( dest => dest.Guid, src => src.ГУИД )
                .Map( dest => dest.Title, src => src.Наименование );

            config.NewConfig<category, Department>()
                .Map( dest => dest.Guid, src => src.GUID )
                .Map( dest => dest.Title, src => src.Name );

            config.NewConfig<subject, Subject>()
                .Map( dest => dest.Guid, src => src.GUIDsubject )
                .Map( dest => dest.Title, src => src.subjectName )
                .Map( dest => dest.Duration, src => src.duration )
                .Map( dest => dest.Attestation, src => String.IsNullOrEmpty( src.Attestation.formControl.GUIDFormControl ) ? null : src.Attestation.formControl );


            config.NewConfig<ФормаКонтроля, Domain.Models.Education.Attestation>()
                .Map( dest => dest.Guid, src => src.ГУИД )
                .Map( dest => dest.Title, src => src.Наименование );

            config.NewConfig<formControl, Domain.Models.Education.Attestation>()
                .Map( dest => dest.Guid, src => src.GUIDFormControl )
                .Map( dest => dest.Title, src => src.Name );




            config.NewConfig<Contact, SendPersonTo1CDTO>()
                .Map(dest => dest.FIO, src => src.Name)
                .Map(
                    dest => dest.Phone, 
                    src => src.Phones() != null ? src.Phones().FirstOrDefault().Value.LeaveJustDigits() : "")
                .Map(
                    dest => dest.Email,
                    src => src.Email() != null ? src.Email().FirstOrDefault().Value.ClearEmail() : "")
                .Map(dest => dest.City, src => src.City() ?? "")
                .Map(dest => dest.Address, src => src.Location() ?? "")
                .Map(dest => dest.Education, src => src.Education() ?? "")
                .Map(dest => dest.Expirience, src => src.Experience() ?? "")
                .Map(dest => dest.Position, src => src.Position() ?? "")
                .Map(dest => dest.BirthDay, src => src.Birthday() ?? DateTime.MinValue)
            ;
        }
    }
}