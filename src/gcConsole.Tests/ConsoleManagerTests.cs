using System;
using System.IO;
using System.Text;
using GeniusCode.Components.Console;
using GeniusCode.Components.Console.Support;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace gcConsole.Tests
{
    [TestClass]
    public class ConsoleManagerTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var p = new RequiredValuesOptionSet();
            var name = p.AddRequiredVariable<string>("n", "");
            var age = p.AddRequiredVariable<int>("a", "");
            var age2 = p.AddRequiredVariable<int>("b", "");
            var age3 = p.AddRequiredVariable<int>("c", "");

            var myArgs = "-n FindThisString".Split(' ');

            p.Parse(myArgs);

            var m = new ConsoleManager(p, "Test");
            var canproceed = m.PerformCanProceed(new StringWriter(), new string[] { });
            Assert.IsFalse(canproceed);
        }

        [TestMethod]
        public void ShouldGoIntoHelpMode()
        {
            var p = new RequiredValuesOptionSet();
            var name = p.AddRequiredVariable<string>("n", "");

            var m = new ConsoleManager(p, "Test", "?", new string[] { "TESTMODE" });
            var sb = new StringBuilder();
            var stringWriter = new StringWriter(sb);
            var canProceed = m.PerformCanProceed(stringWriter, new string[] { "/?" });

            Assert.IsFalse(canProceed);

            var content = sb.ToString();
            Assert.IsTrue(content.Contains("TESTMODE"));
        }

        [TestMethod]
        public void ShouldSupportVisualStudioErrors()
        {
            var p = new RequiredValuesOptionSet();
            var name = p.AddRequiredVariable<string>("n", "");
            var age = p.AddRequiredVariable<int>("a", "");
            var age2 = p.AddRequiredVariable<int>("b", "");
            var age3 = p.AddRequiredVariable<int>("c", "");
                     
            var m = new ConsoleManager(p, "TestConsoleApp");

            var myArgs = "-n FindThisString".Split(' ');

            m.MakeOptionExceptionsVisualStudioAware();

            var writer = new StringWriter();
            var canproceed = m.PerformCanProceed(writer, myArgs);
            Assert.IsFalse(canproceed);

            // Test contains Visual Studio error message
            Assert.IsTrue(writer.ToString().Contains("TestConsoleApp : error"));
        }

    }
}
