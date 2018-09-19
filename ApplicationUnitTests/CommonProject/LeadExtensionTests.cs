using Common.Configuration.Crm;
using Common.Extensions.Models.Crm;
using Domain.Models.Crm;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationUnitTests.CommonProject
{
    [TestClass]
    public class LeadExtensionTests
    {
        [TestMethod]
        public void SourceValue()
        {
            Lead lead = new Lead();
            lead.Fields = new List<Domain.Models.Crm.Fields.Field>
            {
                new Domain.Models.Crm.Fields.Field{ Id = (int)LeadFieldsEnum.Guid, Values = new List<Domain.Models.Crm.Fields.FieldValue>{ new Domain.Models.Crm.Fields.FieldValue { Enum = 111, Value = "ТЕст" } } }
            };

            Assert.AreEqual(lead.Guid(), "ТЕст");
        }

        [TestMethod]
        public void SourceNull()
        {
            Lead lead = new Lead();
            lead.Fields = new List<Domain.Models.Crm.Fields.Field>
            {
                new Domain.Models.Crm.Fields.Field{ Id = (int)LeadFieldsEnum.Guid, Values = new List<Domain.Models.Crm.Fields.FieldValue>{ new Domain.Models.Crm.Fields.FieldValue { Enum = 111 } } }
            };

            Assert.AreEqual(lead.Sources(), null);
        }

        [TestMethod]
        public void SourceFieldsNull()
        {
            Lead lead = new Lead();

            Assert.AreEqual(lead.Sources(), null);
        }
    }
}
