using Common.Configuration.Crm;
using Common.Extensions;
using Common.Extensions.Models.Crm;
using Common.Mapping;
using Domain.Models.Crm;
using LibraryAmoCRM.Configuration;
using LibraryAmoCRM.Models;
using Mapster;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ApplicationUnitTests.CommonProject
{
    [TestClass]
    public class ContactTests
    {
        Contact contact = new Contact();
        Contact mini = new Contact();
        TypeAdapterConfig mapper = new TypeAdapterConfig();

        public ContactTests()
        {
            new Domain_AmoCRM( mapper);

            contact.AccountId = 17769199;
            contact.ClosestTaskAt = DateTime.MinValue;
            contact.CreatedAt = DateTime.Now;
            contact.CreatedBy = 0;
            contact.GroupId = 0;
            contact.Id = 22309159;
            contact.Name = "Иджян Илья Юрьевич";
            contact.ResponsibleUserId = 2079718;
            contact.UpdatedAt = DateTime.Now;
            contact.UpdatedBy = 2079718;

            contact.Tags = new List<Domain.Models.Crm.Fields.Tag> {
                new Domain.Models.Crm.Fields.Tag { Id = 72289, Name = "Заявка с сайта" },
                new Domain.Models.Crm.Fields.Tag { Id = 176263, Name = "callback"}
            };

            contact.Leads = new List<Lead> {
                new Lead { Id = 9982719 },
                new Lead { Id = 10362151 },
                new Lead { Id = 10374575 }
            };

            contact.Company = new Company { Id = 22797025, Name = "АССОЦИАЦИЯ ПРОФЕССИОНАЛОВ ФИТНЕСА" };

            contact.City("Москва"); contact.Agreement(false); contact.Birthday("1978/02/02".ToDateTime('/'));
            contact.Education("Высшее"); contact.Email(EmailTypeEnum.PRIV, "kloder@mail.ru"); contact.Experience("5 лет");
            contact.GroupPart("2"); contact.Guid("c41cb2da-8977-11e6-8102-10c37b94684b"); contact.Location("Avtozavod");
            contact.MailChimp(true); contact.Messenger(MessengerTypeEnum.SKYPE, "kloder1"); contact.Phones(PhoneTypeEnum.MOB, "89031453412");
            contact.Position("Developer");

            contact.Fields = contact.GetChanges().Fields;

            mini.Id = 21966917;
            mini.Name = "Рубцов Роман";
            mini.Email(EmailTypeEnum.PRIV, "roman1895@rambler.ru");

            mini.Fields = mini.GetChanges().Fields;
        }

        [TestMethod]
        public void GetFields()
        {
            Assert.AreEqual(contact.City(), "Москва");
            Assert.AreEqual(contact.Agreement(), false);
            Assert.AreEqual(contact.Birthday(), "1978/02/02".ToDateTime('/'));
            Assert.AreEqual(contact.Education(), "Высшее");
            Assert.AreEqual(contact.Email().FirstOrDefault(i=>i.Key == EmailTypeEnum.PRIV).Value, "kloder@mail.ru");
            Assert.AreEqual(contact.Experience(), "5 лет");
            Assert.AreEqual(contact.GroupPart(), "2");
            Assert.AreEqual(contact.Guid(), "c41cb2da-8977-11e6-8102-10c37b94684b");
            Assert.AreEqual(contact.Location(), "Avtozavod");
            Assert.AreEqual(contact.MailChimp(), true);
            Assert.AreEqual(contact.Messenger().FirstOrDefault(i => i.Key == MessengerTypeEnum.SKYPE).Value, "kloder1");
            Assert.AreEqual(contact.Phones().FirstOrDefault(i => i.Key == PhoneTypeEnum.MOB).Value, "89031453412");
            Assert.AreEqual(contact.Position(), "Developer");
        }

        [TestMethod]
        public void ContactFullDataMapping()
        {
            ContactDTO dto;
            dto = contact.Adapt<ContactDTO>(mapper);

            Assert.IsNotNull(dto);
            Assert.AreEqual(dto.Id, 22309159);

            var contact2 = dto.Adapt<Contact>(mapper);

            Assert.IsInstanceOfType(contact2.Leads, typeof(List<Lead>));
        }

        [TestMethod]
        public void ContactMiniDataMapping()
        {
            ContactDTO dto;
            dto = mini.Adapt<ContactDTO>(mapper);

            Assert.IsNotNull(dto);
            Assert.AreEqual(dto.Id, 21966917);

            var contact2 = dto.Adapt<Contact>(mapper);
        }
    }
}
