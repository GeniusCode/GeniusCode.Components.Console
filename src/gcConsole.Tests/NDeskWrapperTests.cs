using System.Linq;
using GeniusCode.Components.Console.Support;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NDesk.Options;

namespace gcConsole.Tests
{
    [TestClass]
    public class NDeskWrapperTests
    {
        [TestMethod]
        public void Should_get_simple_variables()
        {
            var p = new OptionSet();

            var name = p.AddVariable<string>("n", "");
            var age = p.AddVariable<int>("a", "");

            var myArgs = "-n FindThisString -a:23".Split(' ');
            p.Parse(myArgs);

            Assert.AreEqual(23, age);
            Assert.AreEqual("FindThisString", name);

        }

        [TestMethod]
        public void Should_detect_require_variables()
        {
            var p = new RequiredValuesOptionSet();
            var name = p.AddRequiredVariable<string>("n", "");
            var age = p.AddRequiredVariable<int>("a", "");
            var age2 = p.AddRequiredVariable<int>("b", "");
            var age3 = p.AddRequiredVariable<int>("c", "");

            var myArgs = "-n FindThisString".Split(' ');
            p.Parse(myArgs);

            Assert.AreEqual(3, p.GetMissingVariables().Count());

            Assert.AreEqual("FindThisString", name);
        }


        [TestMethod]
        public void Should_detect_options()
        {
            var p = new OptionSet();

            var n = p.AddFlag("n", "");
            var a = p.AddFlag("a", "");
            var b = p.AddFlag("b", "");

            var myArgs = "-n -a".Split(' ');
            p.Parse(myArgs);

            Assert.IsTrue(n);
            Assert.IsTrue(a);
            Assert.IsFalse(b);
        }


        [TestMethod]
        public void Should_throw_exception_of_setting_variable_twice()
        {
            var exceptionHappened = false;

            var p = new OptionSet();
            var n = p.AddVariable<string>("n", "");

            var myArgs = "-n:Ryan -n:Jer".Split(' ');

            try
            {
                p.Parse(myArgs);
            }
            catch (OptionException ex)
            {
                if (ex.OptionName == "n=")
                    exceptionHappened = true;
            }

            Assert.IsTrue(exceptionHappened);
        }

        [TestMethod]
        public void Should_process_variablelists()
        {
            var p = new OptionSet();

            var n = p.AddVariableList<string>("n", "");
            var a = p.AddVariableList<int>("a", "");

            var myArgs = "-n FindThisString -n:Findit2 -n:Findi3 -a2 -a3 -a5565 -a:23".Split(' ');
            p.Parse(myArgs);

            Assert.AreEqual(3, n.Values.Count());
            Assert.AreEqual(4, a.Values.Count());

            Assert.IsTrue(n.Values.Contains("FindThisString"));
            Assert.IsTrue(a.Values.Contains(23));
        }


        [TestMethod]
        public void Should_process_matricies()
        {
            var p = new OptionSet();

            var n = p.AddVariableMatrix<string>("n", "");

            var myArgs =
                "-n:Hello=World -n:Color=Red \"-n:Message=Hello With Spaces\" -nName=Ryan -nFavNHL:VancouverCanucks".Split('-');
            p.Parse(myArgs.Select(a => "-" + a.Trim()).ToArray());

            Assert.AreEqual(5, n.Matrix.Count());
            Assert.IsTrue(n.Matrix.ContainsKey("Hello"));
            Assert.IsTrue(n.Matrix.ContainsKey("Color"));
            Assert.IsTrue(n.Matrix.ContainsKey("Message"));
            Assert.AreEqual("Ryan", n.Matrix["Name"]);
            Assert.AreEqual("VancouverCanucks", n.Matrix["FavNHL"]);
        }

    }



}
