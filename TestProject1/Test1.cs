using PR6_2.Pages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestProject1
{
    [TestClass]
    public sealed class Test1
    {
        [TestMethod]
        public void TestMethod1()
        {

        }
        [TestMethod]
        public void CalculateW_X0Y0Z0_Returns0()
        {
            double result = 0;
            Exception exception = null;

            var thread = new Thread(() =>
            {
                try
                {
                    var page = new Page1();
                    result = page.Calculate("0", "0", "0");
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();

            if (exception != null)
                throw exception;

            Assert.AreEqual(0, result, 0.0001);
        }
    }
}
