using Domain.Models.Education;
using Mapster;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceLibraryNeoClient.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationUnitTests.Mappings
{
    [TestClass]
    public class Domain_Neo
    {
        TypeAdapterConfig mapper = new TypeAdapterConfig();

        public Domain_Neo()
        {
            new Common.Mapping.Domain_Neo(mapper);
        }

        [TestMethod]
        public void Programs()
        {
            var domain = new EducationProgram();

            domain.Accepted = DateTime.Today;
            domain.Active = true;
            domain.Department = new Department { Guid = "1111-DEPT", Title = "Уч Центр" };
            domain.EducationForm = new EducationForm { Guid = "2222-FORM", Title = "Очная" };
            domain.Guid = "3333-PRGRM";
            domain.Title = "Как тренировать голубей";
            domain.Type = "Курс";
            domain.Variant = "Обучение";
            domain.Subjects = new List<Subject> {
                new Subject { Title = "Урок-1", Guid="444-LSN", Duration="2", Attestation = new Attestation{ Title = "Экамен", Guid = "555-EXMN" } },
                new Subject { Title = "Урок-2", Guid="444-LSN", Duration="2" }
            };

            var nodeDTO = domain.Adapt<ProgramNode>( mapper );

            Assert.AreEqual( nodeDTO.Active, true );
            Assert.AreNotEqual( nodeDTO.Form, null );
            Assert.AreNotEqual( nodeDTO.Department, null );

            var domainBack = nodeDTO.Adapt<EducationProgram>( mapper );

            Assert.AreNotEqual( domainBack.EducationForm, null );
            Assert.AreNotEqual( domainBack.Department, null );
        }
    }
}
