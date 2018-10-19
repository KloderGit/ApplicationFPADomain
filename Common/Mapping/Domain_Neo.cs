using Domain.Models.Education;
using Mapster;
using ServiceLibraryNeoClient.Models;

namespace Common.Mapping
{
    public class Domain_Neo
    {
        public Domain_Neo(TypeAdapterConfig config)
        {
            config.NewConfig<ProgramNode, EducationProgram>()
                    .Map( dest => dest.EducationForm, src => src.Form );

            config.NewConfig<EducationProgram, ProgramNode>()
                    .Map( dest => dest.Form, src => src.EducationForm );

            config.NewConfig<FormNode, EducationForm>();
            config.NewConfig<EducationForm, FormNode>();

            config.NewConfig<DepartmentNode, Department>();
            config.NewConfig<Department, DepartmentNode>();

            config.NewConfig<SubjectNode, Subject>();
            config.NewConfig<Subject, SubjectNode>();

            config.NewConfig<AttestationNode, Attestation>();
            config.NewConfig<Attestation, AttestationNode>();

        }
    }
}