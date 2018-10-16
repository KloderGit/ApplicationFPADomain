using Common.DTO.Service1C;
using Common.Extensions;
using Library1C.DTO;
using Mapster;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiTests
{
    [TestClass]
    public class Mappings
    {
        [TestMethod]
        public void SendLeadto1CDTo_AddLeadDTO()
        {
            var commonDTO = new SendLeadto1CDTo
            {
                ProgramGuid = "7da38f8f-9f71-11e6-80e7-0cc47a4b75cc",
                UserGuid = "cef55369-cd46-11e8-8103-0cc47a4b75cc",
                ContractTitle = "ТстДГВор",
                ContractGroup = "Тест",
                ContractEducationStart = "2018-10-10".ToDateTime('-'),
                ContractEducationEnd = "2018-10-10".ToDateTime('-').AddDays(10),
                ContractExpire = "2018-10-10".ToDateTime('-').AddDays(180),
                ContractPrice = 8000,
                DecreeTitle = "ТстПрикз"
            };

            var serviceDTO = commonDTO.Adapt<AddLeadDTO>();

            Assert.AreEqual(serviceDTO.ProgramGuid, "7da38f8f-9f71-11e6-80e7-0cc47a4b75cc");
            Assert.AreEqual(serviceDTO.ContractSubGroup,"");
            Assert.AreEqual(serviceDTO.ContractEducationEnd, new DateTime(2018,10,20));
        }
    }
}