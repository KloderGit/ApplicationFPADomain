using Common.Configuration.Crm;
using Common.Extensions;
using Common.Extensions.Models.Crm;
using Domain.Models.Crm;
using LibraryAmoCRM.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ApplicationUnitTests.CommonProject
{
    [TestClass]
    public class ContactExtensionTests
    {
        [TestMethod]
        public void PhoneGet()
        {
            Contact item = new Contact();
            item.Id = 123456;

            item.Agreement(true);
            item.Birthday("2015/06/24".ToDateTime('/'));
            item.City("Moscow");
            item.Education("High");
            item.Email(EmailTypeEnum.PRIV, " kloder@mail.ru&");
            item.Experience("5 years");
            item.GroupPart("2");
            item.Guid("1111-2222-3333");
            item.Location("Avtozavod");
            item.MailChimp(false);
            item.Messenger(MessengerTypeEnum.SKYPE, "kloder1");
            item.Phones(PhoneTypeEnum.WORK, "84951453455");
            item.Position("Developer");

            var changes = item.GetChanges();

            Assert.AreEqual(changes.Agreement(), true);

            Assert.AreEqual(changes.Birthday().Value.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture), "2015/06/24");

            Assert.AreEqual(changes.City(), "Moscow");

            Assert.AreEqual(changes.Email().FirstOrDefault(e=>e.Key == EmailTypeEnum.PRIV).Value, "kloder@mail.ru");

        }
    }
}
