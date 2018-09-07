using Domain.Models.Crm;
using Domain.Models.Education;
using LibraryAmoCRM.Models;
using Mapster;
using ServiceLibraryNeoClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebPortalBuisenessLogic.Models.Crm;
using WebPortalBuisenessLogic.Models.DataBase;

namespace WebPortalBuisenessLogic.Utils.Mapster
{
    public class WebPortal_DataBase : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<ProgramNode, EducationProgram>();

            config.NewConfig<FormNode, EducationForm>();

            config.NewConfig<DepartmentNode, Department>();

            config.NewConfig<SubjectNode, Subject>();

            config.NewConfig<ProgramNode, ProgramDTO>()
                .Map(dest => dest.Form, src => src.Form.Title)
                .Map(dest => dest.Department, src => src.Department.Title)
                .Map(dest => dest.Subjects, src => src.Subjects.Select(i=>i.Title))
            ;

        }
    }
}
