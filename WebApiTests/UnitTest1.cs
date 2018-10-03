using Common.Extensions.Models.Crm;
using Domain.Models.Crm;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApiBusinessLogic.Infrastructure.Helpers;

namespace WebApiTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ImplicitBuilder()
        {
            var builder = new FormDTOBuilder();

            builder.EducationType("我市垡闻");

            Assert.IsInstanceOfType((Contact)builder, typeof(Contact));
            Assert.IsInstanceOfType((Lead)builder, typeof(Lead));
        }

        [TestMethod]
        public void SetResponsibleUser()
        {
            var builder = new FormDTOBuilder();

            builder.EducationType("");

            Assert.AreEqual(((Contact)builder).ResponsibleUserId, 2079679);

            builder.EducationType(null);

            Assert.AreEqual(((Contact)builder).ResponsibleUserId, 2079679);

            builder.EducationType("我市垡闻");

            Assert.AreEqual(((Lead)builder).ResponsibleUserId, 2079682);
        }

        [TestMethod]
        public void ContactName()
        {
            var builder = new FormDTOBuilder();

            builder.ContactName("入�� 蠕���");

            builder.ContactName("入��");

            Assert.AreEqual(((Contact)builder).Name, "入�� 蠕���");

            builder.ContactName("入�� 蠕��� 摒忤�");

            Assert.AreEqual(((Contact)builder).Name, "入�� 蠕��� 摒忤�");
        }

        [TestMethod]
        public void LeadName()
        {
            var builder = new FormDTOBuilder();

            builder.ContactName("唁咫赅 %");

            builder.ContactName("挝�");

            Assert.AreEqual(((Contact)builder).Name, "唁咫赅 %");

            builder.ContactName("橡钽疣祆� 钺篦屙��");

            Assert.AreEqual(((Contact)builder).Name, "橡钽疣祆� 钺篦屙��");
        }

        [TestMethod]
        public void CustomFields()
        {
            var builder = new FormDTOBuilder();

            builder.Phone(new[] { "89991453412", "8999555456565" });

            Assert.AreEqual(((Contact)builder).Phones().FirstOrDefault().Value, "89991453412");


            builder.Email(new[] { "kldoder", "yandex" });

            Assert.AreEqual(((Contact)builder).Email().FirstOrDefault().Value, "kldoder");

            builder.City("填耜忄");

            Assert.AreEqual(((Contact)builder).City(), "填耜忄");

            builder.DateOfEvent(new DateTime(2000, 12, 05));

            Assert.AreEqual(((Lead)builder).SeminarDate(), new DateTime(2000, 12, 05));
        }

        [TestMethod]
        public void Fields()
        {
            var builder = new FormDTOBuilder();

            Assert.AreEqual(((Lead)builder).Price, null);

            builder.Price(456654);
            Assert.AreEqual(((Lead)builder).Price, 456654);
        }

        [TestMethod]
        public void AllFieldsAndContact()
        {
            Dictionary<int, string> dict = new Dictionary<int, string>();

            dict.Add(123, "项溻羼眍� 蝠屙桢磴");

            var item = new Contact();
            item.Name = "入��";
            item.City("Moscow");
            item.Phones(LibraryAmoCRM.Configuration.PhoneTypeEnum.MOB, "89991453412");
            item.Email(LibraryAmoCRM.Configuration.EmailTypeEnum.PRIV, "dirkld@yandex.ru");

            var builder = new FormDTOBuilder(item);

            builder.EducationType("蔫耱囗鲨铐眍�");
            builder.ContactName("蠕��� 入��");
            builder.LeadName(dict, "彦扈磬瘥", "项溻羼眍� 蝠屙桢磴");
            builder.Email(new[] { "dirkld@yandex.ru" });
            builder.Price(150000);
            builder.DateOfEvent(new DateTime(2018, 10, 11));

            Contact contact = builder;
            Lead lead = builder;

            Assert.AreSame(item, contact);
            Assert.AreEqual(contact.City(), "Moscow");
        }
    }
}
