using Common.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ApplicationUnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //var str = "2017/10/23".ToDateTime();

            //Assert.IsInstanceOfType(str, typeof(DateTime));

            //var ddd = new DateTime(2017, 10, 23);

            //Assert.AreEqual(ddd, str);

            var str2 = "2017/10/23".ToDateTime(dateSpliter: '/');

            Assert.IsInstanceOfType(str2, typeof(DateTime));

            var ddd2 = new DateTime(2017, 10, 23);

            Assert.AreEqual(ddd2, str2);        

        }
    }
}
